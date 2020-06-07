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
                if(this.Match(source, CLASS))
                {
                    return this.Class(source);
                }

                if(this.Match(source, FUN))
                {
                    return this.Function(source,
                                         "function");
                }

                if (this.Match(source,
                               VAR))
                {
                    return this.VarDecl(source);
                }

                return this.Statement(source);
            }
            catch (ParseError)
            {
                this.Synchronize(source);
                return new Expression(new Literal(null));
            }
        }

        private Stmt Class(in ScannedSource source)
        {
            Token name = this.Consume(source,
                                      IDENTIFIER,
                                      "Expect name after class declaration.");
            this.Consume(source,
                         LEFT_BRACE,
                         "Expect '{' after class name.");

            var methods = new List<Stmt.Function>();

            while (!this.Check(source,
                               RIGHT_BRACE)
                   && !this.IsAtEnd(source))
            {
                methods.Add(this.Function(source,
                                          "method"));
            }

            this.Consume(source,
                         RIGHT_BRACE,
                         "Expect '}' after class methods");

            return new Class(name,
                             methods);
        }

        private Stmt.Function Function(in ScannedSource source, 
                                       string kind)
        {
            Token name = this.Consume(source,
                                      IDENTIFIER,
                                      $"Expect {kind} name.");

            this.Consume(source,
                         LEFT_PAREN,
                         $"Expect '(' after {kind} name.");

            List<Token> parameters = new List<Token>();

            if(!this.Check(source, RIGHT_PAREN))
            {
                do
                {
                    if(parameters.Count >= Limits.MaxArguments)
                    {
                        this.Error(source,
                                   this.Peek(source),
                                   $"{kind} cannot have more than {Limits.MaxArguments} parameters.");
                    }

                    parameters.Add(this.Consume(source,
                                                IDENTIFIER,
                                                "Expect parameter name."));
                }
                while (this.Match(source,
                                  COMMA));
            }

            this.Consume(source,
                         RIGHT_PAREN,
                         $"Expect ')' after {kind} parameter list.");

            this.Consume(source,
                         LEFT_BRACE,
                         $"Expect '{{' after {kind} parameters");

            Block body = this.BlockStatement(source);

            return new Function(name,
                                parameters,
                                body.statements);
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
            if(this.Match(source, FOR))
            {
                return this.ForStatement(source);
            }

            if (this.Match(source, IF))
            {
                return this.IfStatement(source);
            }

            if(this.Match(source, WHILE))
            {
                return this.WhileStatement(source);
            }

            if (this.Match(source,
                           PRINT))
            {
                return this.PrintStatement(source);
            }

            if(this.Match(source, 
                          RETURN))
            {
                return this.ReturnStatement(source);
            }

            if (this.Match(source,
                           LEFT_BRACE))
            {
                return this.BlockStatement(source);
            }

            return this.ExpressionStatement(source);
        }

        private Stmt ForStatement(in ScannedSource source)
        {
            this.Consume(source,
                         LEFT_PAREN,
                         "Expect '(' after for statement.");

            Stmt? initializer = null;

            if (!this.Match(source,
                            SEMICOLON))
            {
                if (this.Match(source,
                               VAR))
                {
                    initializer = this.VarDecl(source);
                }
                else
                {
                    initializer = this.ExpressionStatement(source);
                }
            }

            Expr? condition = null;

            if (!this.Check(source,
                            SEMICOLON))
            {
                condition = this.Expression(source);
            }

            this.Consume(source,
                         SEMICOLON,
                         "Expect ';' after for condition.");

            Expr? increment = null;

            if (!this.Check(source,
                            RIGHT_PAREN))
            {
                increment = this.Expression(source);
            }

            this.Consume(source,
                         RIGHT_PAREN,
                         "Expect ')' after for increment.");

            Stmt body = this.Statement(source);

            if (increment != null)
            {
                body = new Block(new List<Stmt>
                                 {
                                     body,
                                     new Expression(increment)
                                 });
            }

            if (condition == null)
            {
                condition = new Literal(true);
            }

            body = new While(condition,
                             body);

            if (initializer != null)
            {
                body = new Block(new List<Stmt>
                                 {
                                     initializer,
                                     body
                                 });
            }

            return body;
        }

        private Stmt IfStatement(in ScannedSource source)
        {
            this.Consume(source,
                         LEFT_PAREN,
                         "Expect '(' after if statement.");

            Expr condition = this.Expression(source);

            this.Consume(source,
                         RIGHT_PAREN,
                         "Expect ')' after if condition.");
            Stmt thenStatement = this.Statement(source);

            Stmt? elseStatement = null;

            if (this.Match(source,
                           ELSE))
            {
                elseStatement = this.Statement(source);
            }

            return new If(condition,
                          thenStatement,
                          elseStatement);
        }

        private Stmt WhileStatement(in ScannedSource source)
        {
            this.Consume(source,
                         LEFT_PAREN,
                         "Expect '(' after while.");

            Expr condition = this.Expression(source);

            this.Consume(source,
                         RIGHT_PAREN,
                         "Expect ')' after while expression.");

            Stmt statement = this.Statement(source);

            return new While(condition,
                             statement);
        }

        private Stmt PrintStatement(in ScannedSource source)
        {
            Expr value = this.Expression(source);
            this.Consume(source,
                         SEMICOLON,
                         "Expect ';' after value");
            return new Print(value);
        }

        private Stmt ReturnStatement(in ScannedSource source)
        {
            Token keyword = this.Previous(source);
            Expr? value = null;

            if(!this.Check(source, SEMICOLON))
            {
                value = this.Expression(source);
            }

            this.Consume(source,
                         SEMICOLON,
                         "Expect ';' after return statement.");

            return new Return(keyword,
                              value);
        }

        private Block BlockStatement(in ScannedSource source)
        {
            var statements = new List<Stmt>();

            while (!this.Check(source,
                               RIGHT_BRACE)
                   && !this.IsAtEnd(source))
            {
                statements.Add(this.Declaration(source));
            }

            this.Consume(source,
                         RIGHT_BRACE,
                         "Expect '}' at end of block.");
                 
            return new Block(statements);
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
                return new Print(expr);
            }

            throw this.Error(source,
                             this.Peek(source),
                             "Expect ';' after expression.");
        }

        private Expr Expression(in ScannedSource source) => this.Assignment(source);

        private Expr Assignment(in ScannedSource source)
        {
            Expr expr = this.LogicOr(source);

            if(this.Match(source, EQUAL))
            {
                Token equals = this.Previous(source);
                Expr value = this.Assignment(source);

                if(expr is Variable variable)
                {
                    Token name = variable.name;
                    return new Assign(name,
                                      value);
                }

                this.Error(source,
                           equals,
                           "Invalid assignment target.");
            }

            return expr;
        }

        private Expr LogicOr(in ScannedSource source)
        {
            Expr expr = this.LogicAnd(source);

            while (this.Match(source,
                              OR))
            {
                Token op = this.Previous(source);
                Expr right = this.LogicAnd(source);
                expr = new Logical(expr,
                                   op,
                                   right);
            }

            return expr;
        }

        private Expr LogicAnd(in ScannedSource source)
        {
            Expr expr = this.Equality(source);

            while (this.Match(source,
                              AND))
            {
                Token op = this.Previous(source);
                Expr right = this.Equality(source);
                expr = new Logical(expr,
                                   op,
                                   right);
            }

            return expr;
        }

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

            return this.Call(source);
        }

        private Expr Call(in ScannedSource source)
        {
            Expr expr = this.Primary(source);

            while (true)
            {
                if(this.Match(source, LEFT_PAREN))
                {
                    expr = this.FinishCall(expr,
                                           source);
                }
                else
                {
                    break;
                }
            }

            return expr;
        }

        private Expr FinishCall(Expr callee, 
                                in ScannedSource source)
        {
            var arguments = new List<Expr>();

            if (!this.Check(source,
                            RIGHT_PAREN))
            {
                do
                {
                    arguments.Add(this.Expression(source));

                    if(arguments.Count > Limits.MaxArguments)
                    {
                        this.Error(source,
                                   this.Peek(source),
                                   "A function cannot have more than 255 arguments.");
                    }
                }
                while (this.Match(source,
                                  COMMA));
            }

            Token paren = this.Consume(source,
                                       RIGHT_PAREN,
                                       "Expect ')' after function call arguments");

            return new Call(callee,
                            paren,
                            arguments);
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

            if(this.Match(source, IDENTIFIER))
            {
                return new Variable(this.Previous(source));
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
            ParseError parseError = new ParseError(message);
            this.parseErrors.Add(parseError);
            return parseError;
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