using System;
using System.Collections.Generic;
using System.Linq;
using lox.constants;

using static lox.constants.TokenType;
using static lox.Expr;
using static lox.Stmt;

namespace lox
{
    public class Parser
    {
        #region Fields

        private int current;

        private readonly List<ParseError> parseErrors = new List<ParseError>();

        #endregion

        #region Instance Methods

        public List<Stmt> Parse(in ScannedSource source)
        {
            this.current = 0;
            this.parseErrors.Clear();

            var declarations = new List<Stmt>();

            while (!this.IsAtEnd(source))
            {
                declarations.Add(this.Declaration(source));
            }

            return declarations;
        }

        #endregion

        #region Instance Properties

        /// <summary>
        /// Collects any errors from the most recent parse.
        /// </summary>
        public IEnumerable<ParseError> ParseErrors => this.parseErrors;

        #endregion

        #region GrammarRules

        private Stmt Declaration(in ScannedSource source)
        {
            try
            {
                if (this.Match(source,
                               VAR))
                {
                    return this.VarDecl(source);
                }

                return this.Statement(source);
            }
            catch (ParseError parseError)
            {
                this.Synchronize(source);
                this.parseErrors.Add(parseError);
                return new Expression(new Literal(null));
            }
        }

        private Stmt VarDecl(in ScannedSource source)
        {
            var identifier = this.Consume(source,
                                          IDENTIFIER,
                                          "Expect identifier after 'var'.");

            Expr? initializer = null;

            if(this.Match(source, EQUAL))
            {
                initializer = this.Expression(source);
            }

            this.Consume(source,
                         SEMICOLON,
                         "Expect ';' after variable declaration.");

            return new Var(identifier,
                           initializer);
        }

        private Stmt Statement(in ScannedSource source)
        {
            if (this.Match(source,
                           PRINT))
            {
                return this.PrintStatement(source);
            }

            return this.ExpressionStatement(source);
        }

        private Stmt PrintStatement(in ScannedSource source)
        {
            Expr value = this.Expression(source);
            this.Consume(source,
                         SEMICOLON,
                         "Expect ';' after value");
            return new Print(value);
        }

        private Stmt ExpressionStatement(in ScannedSource source)
        {
            Expr expr = this.Expression(source);

            if(this.Match(source, SEMICOLON))
            {
                return new Expression(expr);
            }

            //TODO: This is not quite right. I am attempting to support expression
            //evaluation at the REPL without a semicolon. This works, but it also
            //allows the final statement in a sequence of loose statements to appear
            //without a semicolon.
            if (this.IsAtEnd(source))
            {
                return new Expression(expr);
            }

            throw this.Error(source,
                             this.Peek(source),
                             "Expect ';' after expression.");
        }

        private Expr Expression(in ScannedSource source) => this.Equality(source);

        private Expr Equality(in ScannedSource source)
        {
            Expr expr = this.Comparison(source);

            while (this.Match(source,
                              EQUAL_EQUAL,
                              BANG_EQUAL))
            {
                Token op = this.Previous(source);
                Expr right = this.Comparison(source);
                expr = new Binary(expr,
                                  op,
                                  right);
            }

            return expr;
        }

        private Expr Comparison(in ScannedSource source)
        {
            Expr expr = this.Addition(source);

            while (this.Match(source,
                              GREATER,
                              GREATER_EQUAL,
                              LESS,
                              LESS_EQUAL))
            {
                Token op = this.Previous(source);
                Expr right = this.Addition(source);
                expr = new Binary(expr,
                                  op,
                                  right);
            }

            return expr;
        }

        private Expr Addition(in ScannedSource source)
        {
            Expr expr = this.Multiplication(source);

            while (this.Match(source,
                              PLUS,
                              MINUS))
            {
                Token op = this.Previous(source);
                Expr right = this.Multiplication(source);
                expr = new Binary(expr,
                                  op,
                                  right);
            }

            return expr;
        }

        private Expr Multiplication(in ScannedSource source)
        {
            Expr expr = this.Unary(source);

            while (this.Match(source,
                              STAR,
                              SLASH))
            {
                Token op = this.Previous(source);
                Expr right = this.Unary(source);
                expr = new Binary(expr,
                                  op,
                                  right);
            }

            return expr;
        }

        private Expr Unary(in ScannedSource source)
        {
            if (this.Match(source,
                           BANG,
                           MINUS))
            {
                var op = this.Previous(source);
                return new Unary(op,
                                 this.Unary(source));
            }

            return this.Primary(source);
        }

        private Expr Primary(in ScannedSource source)
        {
            if (this.Match(source,
                           FALSE))
            {
                return new Literal(false);
            }

            if (this.Match(source,
                           TRUE))
            {
                return new Literal(true);
            }

            if (this.Match(source,
                           NIL))
            {
                return new Literal(null);
            }

            if (this.Match(source,
                           NUMBER,
                           STRING))
            {
                return new Literal(this.Previous(source)
                                       .Literal);
            }

            if (this.Match(source,
                           LEFT_PAREN))
            {
                Expr expr = this.Expression(source);
                this.Consume(source,
                             RIGHT_PAREN,
                             "Expect ')' after grouping expression.");
                return new Grouping(expr);
            }

            throw this.Error(source,
                             this.Peek(source),
                             "Expect expression");
        }

        #endregion

        #region Token operations

        private Token Consume(in ScannedSource source,
                              TokenType tokenType,
                              string error)
        {
            if (this.Check(source,
                           tokenType))
            {
                return this.Advance(source);
            }

            throw this.Error(source,
                             this.Peek(source),
                             error);
        }

        private bool Match(in ScannedSource source,
                           params TokenType[] tokenTypes)
        {
            for (int i = 0;
                 i < tokenTypes.Length;
                 i++)
            {
                if (this.Check(source,
                               tokenTypes[i]))
                {
                    this.Advance(source);
                    return true;
                }
            }

            return false;
        }

        private bool Check(in ScannedSource source,
                           TokenType tokenType)
        {
            if (this.IsAtEnd(source))
            {
                return false;
            }

            return this.Peek(source)
                       .Type
                   == tokenType;
        }

        private Token Advance(in ScannedSource source)
        {
            if (!this.IsAtEnd(source))
            {
                this.current++;
            }

            return this.Previous(source);
        }

        private bool IsAtEnd(in ScannedSource source)
        {
            return this.Peek(source)
                       .Type
                   == EOF;
        }

        private Token Peek(in ScannedSource source)
        {
            return source.Tokens[this.current];
        }

        private Token Previous(in ScannedSource source)
        {
            return source.Tokens[this.current - 1];
        }

        #endregion

        #region Error Handling

        private ParseError Error(in ScannedSource source,
                                 Token token,
                                 string message)
        {
            Lox.Error(source,
                      token,
                      message);
            return new ParseError(message);
        }

        /// <summary>
        /// After encountering a serious parse error, discard tokens until we reach
        /// a statement or other well-defined boundary to resume parsing. This attempts
        /// to maximize the unique errors we can report to the user without reporting
        /// duplicate errors.
        /// </summary>
        private void Synchronize(in ScannedSource source)
        {
            this.Advance(source);

            while (!this.IsAtEnd(source))
            {
                if(this.Previous(source).Type == SEMICOLON)
                {
                    return;
                }

                switch (this.Peek(source)
                            .Type)
                {
                    case CLASS:
                    case FUN:
                    case VAR:
                    case FOR:
                    case IF:
                    case WHILE:
                    case PRINT:
                    case RETURN:
                        return;
                }

                this.Advance(source);
            }
        }

        #endregion
    }
}