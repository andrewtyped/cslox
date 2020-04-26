using System;

using lox.constants;

using static lox.constants.TokenType;
using static lox.Expr;

namespace lox
{
    public class Parser
    {
        #region Fields

        private int current;

        #endregion

        #region Instance Methods

        public Expr Parse(in ScannedSource source)
        {
            return this.Expression(source);
        }

        #endregion

        #region GrammarRules

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

            return null!;
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

            throw new NotImplementedException("Pick this up later");
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
    }
}