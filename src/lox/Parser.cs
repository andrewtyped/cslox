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
            return this.Primary(source);
        }

        private Token Advance(in ScannedSource source)
        {
            if (!this.IsAtEnd(source))
            {
                this.current++;
            }

            return this.Previous(source);
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

        private bool IsAtEnd(in ScannedSource source)
        {
            return this.Peek(source)
                       .Type
                   == EOF;
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

        private Token Peek(in ScannedSource source)
        {
            return source.Tokens[this.current];
        }

        private Token Previous(in ScannedSource source)
        {
            return source.Tokens[this.current - 1];
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

            if(this.Match(source, NIL))
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

            //TODO: Support expression!

            return null!;
        }

        #endregion
    }
}