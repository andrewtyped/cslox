﻿using System;
using System.Collections.Generic;

using lox.constants;

using static lox.constants.TokenType;

using SpStr = System.ReadOnlySpan<char>;

namespace lox
{
    /// <summary>
    /// Converts a string of Lox code to a list of tokens.
    /// </summary>
    public class Scanner
    {
        #region Fields

        private static readonly IReadOnlyDictionary<string, TokenType> keywords = new Dictionary<string, TokenType>()
        {
            {"and", AND },
            {"class", CLASS },
            {"else", ELSE },
            {"false", FALSE },
            {"for", FOR },
            {"fun", FUN },
            {"if", IF },
            {"nil", NIL },
            {"or", OR },
            {"print", PRINT },
            {"return", RETURN },
            {"super", SUPER },
            {"this", THIS },
            {"true", TRUE },
            {"var", VAR },
            {"while", WHILE }
        };

        private readonly List<Token> tokens = new List<Token>();

        private int current;

        private int line = 1;

        private int start;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance.
        /// </summary>
        public Scanner()
        {
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Reads the content of <see cref="Source"/> to produce a list of Lox tokens.
        /// </summary>
        /// <returns>The list of tokens read from <see cref="Source"/></returns>
        public IReadOnlyList<Token> ScanTokens(ReadOnlySpan<char> source)
        {
            if (this.tokens.Count > 0)
            {
                return this.tokens;
            }

            while (!this.IsAtEnd(source))
            {
                //We are at the beginning of the next lexeme.
                this.start = this.current;
                this.ScanToken(source);
            }

            this.tokens.Add(new Token(this.current,
                                      this.current,
                                      this.line,
                                      EOF));

            return this.tokens;
        }

        private void AddToken(TokenType tokenType)
        {
            this.tokens.Add(new Token(this.start,
                                      this.current,
                                      this.line,
                                      tokenType));
        }

        private void AddToken(TokenType tokenType,
                              object literal)
        {
            this.tokens.Add(new Token(this.start,
                                      this.current,
                                      this.line,
                                      literal,
                                      tokenType));
        }

        private char Advance(in SpStr source)
        {
            this.current++;
            return source[this.current - 1];
        }

        private void Identifier(in SpStr source)
        {
            while (this.IsAlphaNumeric(this.Peek(source)))
            {
                this.Advance(source);
            }

            //HACK: Lame attempt to avoid allocations. Longest keyword is 'return' at length 6. Shortest is 'or'/'if' at 2. Any identifier longer or shorter can't be a keyword.
            const int maxKeywordlength = 6;
            const int minKeywordLength = 2;
            int identifierLength = this.current - this.start;
            if (identifierLength > maxKeywordlength || identifierLength < minKeywordLength)
            {
                this.AddToken(IDENTIFIER);
                return;
            }

            //HACK: This tears down all our careful work to avoid string allocations. The book uses a dictionary, I'd need to make substantial changes (like using a trie) to fix this.
            if (keywords.TryGetValue(source.Slice(this.start,
                                                  identifierLength)
                                           .ToString(),
                                     out TokenType tokenType))
            {
                this.AddToken(tokenType);
            }
            else
            {
                this.AddToken(IDENTIFIER);
            }
        }

        private bool IsAlpha(char c)
        {
            return c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z' || c == '_';
        }

        private bool IsAlphaNumeric(char c)
        {
            return this.IsAlpha(c) || this.IsDigit(c);
        }

        private bool IsAtEnd(in SpStr source) => this.current >= source.Length;

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool Match(char expected,
                           in SpStr source)
        {
            if (this.IsAtEnd(source))
            {
                return false;
            }

            if (source[this.current] != expected)
            {
                return false;
            }

            this.current++;
            return true;
        }

        private void Number(in SpStr source)
        {
            while (this.IsDigit(this.Peek(source)))
            {
                this.Advance(source);
            }

            if (this.Peek(source) == '.'
                && this.IsDigit(this.PeekNext(source)))
            {
                this.Advance(source);

                while (this.IsDigit(this.Peek(source)))
                {
                    this.Advance(source);
                }
            }

            this.AddToken(NUMBER,
                          double.Parse(source.Slice(this.start,
                                                    this.current - this.start)));
        }

        private char Peek(in SpStr source)
        {
            if (this.IsAtEnd(source))
            {
                return '\0';
            }

            return source[this.current];
        }

        private char PeekNext(in SpStr source)
        {
            if (this.current + 1 >= source.Length)
            {
                return '\0';
            }

            return source[this.current + 1];
        }

        private void ScanToken(in SpStr source)
        {
            char c = this.Advance(source);

            switch (c)
            {
                //Single Characters
                //=================

                case '(':
                    this.AddToken(LEFT_PAREN);
                    break;
                case ')':
                    this.AddToken(RIGHT_PAREN);
                    break;
                case '{':
                    this.AddToken(LEFT_BRACE);
                    break;
                case '}':
                    this.AddToken(RIGHT_BRACE);
                    break;
                case ',':
                    this.AddToken(COMMA);
                    break;
                case '.':
                    this.AddToken(DOT);
                    break;
                case '-':
                    this.AddToken(MINUS);
                    break;
                case '+':
                    this.AddToken(PLUS);
                    break;
                case ';':
                    this.AddToken(SEMICOLON);
                    break;
                case '*':
                    this.AddToken(STAR);
                    break;

                //Operators
                //=========

                case '!':
                    this.AddToken(this.Match('=',
                                             source)
                                      ? BANG_EQUAL
                                      : BANG);
                    break;
                case '=':
                    this.AddToken(this.Match('=',
                                             source)
                                      ? EQUAL_EQUAL
                                      : EQUAL);
                    break;
                case '<':
                    this.AddToken(this.Match('=',
                                             source)
                                      ? LESS_EQUAL
                                      : LESS);
                    break;
                case '>':
                    this.AddToken(this.Match('=',
                                             source)
                                      ? GREATER_EQUAL
                                      : GREATER);
                    break;
                case '/':
                    if (this.Match('/',
                                   source))
                    {
                        //Discard comments
                        while (this.Peek(source) != '\n'
                               && !this.IsAtEnd(source))
                        {
                            this.Advance(source);
                        }
                    }
                    else
                    {
                        this.AddToken(SLASH);
                    }

                    break;

                //Whitespace
                //==========

                case ' ':
                case '\r':
                case '\t':
                    //Ignore whitespace.
                    break;
                case '\n':
                    this.line++;
                    break;

                //Strings
                //=======

                case '"':
                    this.String(source);
                    break;
                default:

                    //Numbers
                    //=======

                    if (this.IsDigit(c))
                    {
                        this.Number(source);
                    }

                    //Identifiers
                    //===========

                    else if (this.IsAlpha(c))
                    {
                        this.Identifier(source);
                    }

                    else
                    {
                        Lox.Error(this.line,
                                  $"Unexpected character {c}.");
                    }

                    break;
            }
        }

        private void String(in SpStr source)
        {
            while (this.Peek(source) != '"'
                   && !this.IsAtEnd(source))
            {
                if (this.Advance(source) == '\n')
                {
                    this.line++;
                }
            }

            //Unterminated string.
            if (this.IsAtEnd(source))
            {
                Lox.Error(this.line,
                          "Unterminated string");
            }

            //The closing ".
            this.Advance(source);

            //Trim quotes.
            string value = source.Slice(this.start + 1,
                                        this.current - this.start - 2)
                                 .ToString();

            this.AddToken(STRING,
                          value);
        }

        #endregion
    }
}