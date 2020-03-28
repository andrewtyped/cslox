using System;
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

        private readonly List<Token> tokens = new List<Token>();

        private int current;

        private int line = 1;

        private int start;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="source">The Lox code to tokenize.</param>
        public Scanner(string source) => this.Source = source;

        #endregion

        #region Instance Properties

        /// <summary>
        /// Gets the Lox code to tokenize.
        /// </summary>
        public string Source
        {
            get;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Reads the content of <see cref="Source"/> to produce a list of Lox tokens.
        /// </summary>
        /// <returns>The list of tokens read from <see cref="Source"/></returns>
        public IReadOnlyList<Token> ScanTokens()
        {
            if (this.tokens.Count > 0)
            {
                return this.tokens;
            }

            var source = this.Source.AsSpan();

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

        private bool IsAtEnd(in SpStr source) => this.current >= source.Length;

        private bool Match(char expected, in SpStr source)
        {
            if (this.IsAtEnd(source))
            {
                return false;
            }

            if(source[this.current] != expected)
            {
                return false;
            }

            this.current++;
            return true;
        }

        private char Peek(in SpStr source)
        {
            if (this.IsAtEnd(source))
            {
                return '\0';
            }

            return source[this.current];
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
                    if (this.Match('/', source))
                    {
                        //Discard comments
                        while(this.Peek(source) != '\n' && !this.IsAtEnd(source))
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
                default:
                    Lox.Error(line,
                              $"Unexpected character {c}.");
                    break;
            }
        }

        #endregion
    }
}