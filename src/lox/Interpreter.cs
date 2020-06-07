using lox.loxFunctions;
using System;
using System.Collections.Generic;

using static lox.constants.TokenType;

namespace lox
{
    /// <summary>
    /// Evaluates a Lox AST and produces its value.
    /// </summary>
    public class Interpreter : Expr.IVisitor<object?>,
                               Stmt.IVisitor<Void>
    {
        #region Fields

        private IConsole console;

        private readonly Environment globals;

        private readonly Dictionary<Expr, int> locals;

        #endregion

        #region Constructors

        public Interpreter(IConsole console)
        {
            this.console = console ?? throw new ArgumentNullException(nameof(console));
            this.locals = new Dictionary<Expr, int>();
            this.globals = new Environment();
            this.globals.Define("clock",
                                new Token(0,
                                          5,
                                          0,
                                          IDENTIFIER),
                                new Clock(new DateTimeOffsetProvider()));
            this.Environment = this.globals;
        }

        public Interpreter()
            : this(new ConsoleWrapper())
        {
        }

        #endregion

        #region Instance Properties

        internal Environment Environment
        {
            get;
            private set;
        }

        internal IReadOnlyDictionary<Expr, int> Locals => this.locals;

        /// <summary>
        /// Gets the last error encountered while interpreting.
        /// </summary>
        public RuntimeError? LastError
        {
            get;
            private set;
        }

        #endregion

        #region Instance Methods

        public void Interpret(List<Stmt> stmts,
                              in ReadOnlySpan<char> source)
        {
            try
            {
                this.LastError = null;
                
                for (int i = 0;
                     i < stmts.Count;
                     i++)
                {
                    stmts[i]
                        .Accept(this,
                                source);
                }
            }
            catch (RuntimeError runtimeError)
            {
                this.LastError = runtimeError;
                Lox.RuntimeError(runtimeError);
            }
        }

        internal void Resolve(Expr expr, int depth)
        {
            this.locals[expr] = depth;
        }

        #endregion

        #region Statement visitors

        public Void VisitClassStmt(Stmt.Class stmt,
                                   in ReadOnlySpan<char> source)
        {
            this.Environment.Define(source, stmt.name, null);
            LoxClass @class = new LoxClass(stmt.name.GetLexeme(source)
                                               .ToString());
            this.Environment.Assign(source,
                                    stmt.name,
                                    @class);
            return default;
        }

        public Void VisitFunctionStmt(Stmt.Function stmt, 
                                      in ReadOnlySpan<char> source)
        {
            LoxFunction loxFunction = new LoxFunction(source,
                                                      stmt,
                                                      this.Environment);
            Environment.Define(source,
                               stmt.name,
                               loxFunction);
            return default;
        }

        public Void VisitIfStmt(Stmt.If stmt, in ReadOnlySpan<char> source)
        {
            object? condition = this.Evaluate(stmt.condition,
                                              source);

            if (this.IsTruthy(condition,
                              source))
            {
                return stmt.thenBranch.Accept(this,
                                              source);
            }
            else if(stmt.elseBranch != null)
            {
                return stmt.elseBranch.Accept(this,
                                              source);
            }

            return default;
        }

        public Void VisitBlockStmt(Stmt.Block stmt,
                                   in ReadOnlySpan<char> source)
        {
            this.ExecuteBlock(stmt.statements,
                              new Environment(this.Environment),
                              source);

            return default;
        }

        public void ExecuteBlock(List<Stmt> stmts,
                                  Environment environment,
                                  in ReadOnlySpan<char> source)
        {
            var previousEnvironment = this.Environment;
            try
            {
                this.Environment = environment;

                for (int i = 0;
                     i < stmts.Count;
                     i++)
                {
                    stmts[i]
                        .Accept(this,
                                source);
                }
            }
            finally
            {
                this.Environment = previousEnvironment;
            }
        }

        public Void VisitExpressionStmt(Stmt.Expression stmt,
                                        in ReadOnlySpan<char> source)
        {
            var value = this.Evaluate(stmt.expression,
                                      source);

            return default;
        }

        public Void VisitPrintStmt(Stmt.Print stmt,
                                   in ReadOnlySpan<char> source)
        {
            var value = this.Evaluate(stmt.expression,
                                      source);

            this.console.WriteLine(this.Stringify(value));
            return default;
        }

        public Void VisitReturnStmt(Stmt.Return stmt,
                                    in ReadOnlySpan<char> source)
        {
            object? value = null;

            if(stmt.value != null)
            {
                value = stmt.value.Accept(this,
                                          source);
            }

            throw new ReturnValue(value);
        }

        public Void VisitVarStmt(Stmt.Var stmt,
                                 in ReadOnlySpan<char> source)
        {
            object? value = null;

            if(stmt.initializer != null)
            {
                value = this.Evaluate(stmt.initializer,
                                      source);
            }

            this.Environment.Define(source,
                                    stmt.name,
                                    value);

            return default;
        }

        public Void VisitWhileStmt(Stmt.While stmt,
                                   in ReadOnlySpan<char> source)
        {
            while (this.IsTruthy(this.Evaluate(stmt.condition,
                                               source),
                                 source))
            {
                stmt.statement.Accept(this,
                                      source);
            }

            return default;
        }

        #endregion

        #region Expression visitors

        public object? VisitVariableExpr(Expr.Variable expr,
                                         in ReadOnlySpan<char> source)
        {
            return this.LookupVariable(expr,
                                       source);
        }

        public object? VisitAssignExpr(Expr.Assign expr,
                                       in ReadOnlySpan<char> source)
        {
            object? value = this.Evaluate(expr.value,
                                          source);

            if(this.locals.TryGetValue(expr, 
                                       out int distance))
            {
                this.Environment.AssignAt(source,
                                          expr.name,
                                          value,
                                          distance);
            }
            else
            {
                this.globals.Assign(source,
                                    expr.name,
                                    value);
            }

            return value;
        }

        public object? VisitBinaryExpr(Expr.Binary expr,
                                       in ReadOnlySpan<char> source)
        {
            object? left = this.Evaluate(expr.left,
                                         source);
            object? right = this.Evaluate(expr.right,
                                          source);

            double? CalculateLocal(Func<double, double, double> calculation)
            {
                return this.Calculate(expr.op,
                                      left,
                                      right,
                                      calculation);
            }

            bool CompareLocal(Func<double, double, bool> comparison)
            {
                return this.Compare(expr.op,
                                    left,
                                    right,
                                    comparison);
            }

            return expr.op.Type switch
            {
                EQUAL_EQUAL => this.IsEqual(left,
                                            right),
                BANG_EQUAL => !this.IsEqual(left,
                                            right),
                GREATER => CompareLocal((a,
                                         b) => a > b),
                LESS => CompareLocal((a,
                                      b) => a < b),
                GREATER_EQUAL => CompareLocal((a,
                                               b) => a >= b),
                LESS_EQUAL => CompareLocal((a,
                                            b) => a <= b),
                MINUS => CalculateLocal((a,
                                         b) => a - b),
                SLASH => CalculateLocal((a,
                                         b) => a / b),
                STAR => CalculateLocal((a,
                                        b) => a * b),
                PLUS => this.VisitBinaryPlusOperands(expr.op,
                                                     left,
                                                     right),
                _ => null
            };
        }

        public object? VisitCallExpr(Expr.Call expr,
                                     in ReadOnlySpan<char> source)
        {
            object? callee = this.Evaluate(expr.callee,
                                           source);

            if(callee is null)
            {
                throw new RuntimeError(expr.paren,
                                       "Cannot call a nil expression");
            }

            List<object?> arguments = new List<object?>();

            for(int i = 0; i < expr.arguments.Count; i++)
            {
                arguments.Add(this.Evaluate(expr.arguments[i],
                                            source));
            }

            if (callee is ILoxCallable function)
            {
                if (arguments.Count != function.Arity())
                {
                    throw new RuntimeError(expr.paren,
                                           $"Expected {function.Arity()} arguments but got {arguments.Count}.");
                }

                try
                {
                    return function.Call(this,
                                         arguments,
                                         source);
                }
                catch(ReturnValue returnValue)
                {
                    return returnValue.Value;
                }
            }

            throw new RuntimeError(expr.paren,
                                   "Can only call functions and classes.");
        }

        public object? VisitGetExpr(Expr.Get expr,
                                    in ReadOnlySpan<char> source)
        {
            throw new NotImplementedException();
        }

        private object? VisitBinaryPlusOperands(Token op,
                                                object? left,
                                                object? right)
        {
            if (left is null
                || right is null)
            {
                return null;
            }

            if (left is double doubleLeft
                && right is double doubleRight)
            {
                return doubleLeft + doubleRight;
            }

            if (left is string stringLeft
                && right is string stringRight)
            {
                return stringLeft + stringRight;
            }

            throw new RuntimeError(op,
                                   "Operands must be two numbers or two strings.");
        }

        public object? VisitGroupingExpr(Expr.Grouping expr,
                                         in ReadOnlySpan<char> source)
        {
            return this.Evaluate(expr.expression,
                                 source);
        }

        public object? VisitLiteralExpr(Expr.Literal expr,
                                        in ReadOnlySpan<char> source)
        {
            return expr.value;
        }

        public object? VisitLogicalExpr(Expr.Logical expr,
                                       in ReadOnlySpan<char> source)
        {
            object? left = this.Evaluate(expr.left,
                                         source);


            if (expr.op.Type == OR
                && this.IsTruthy(left,
                                 source))
            {
                return left;

            }

            if (expr.op.Type == AND && !this.IsTruthy(left,
                                                      source))
            {
                return left;
            }

            return this.Evaluate(expr.right,
                                 source);
        }

        public object? VisitUnaryExpr(Expr.Unary expr,
                                      in ReadOnlySpan<char> source)
        {
            object? right = this.Evaluate(expr.right,
                                          source);

            return expr.op.Type switch
            {
                MINUS => this.Negate(expr.op,
                                     right),
                BANG => !this.IsTruthy(right,
                                       source),
                _ => null
            };
        }

        #endregion

        #region Utilities

        private object? LookupVariable(Expr.Variable expr,
                                       in ReadOnlySpan<char> source)
        {
            if (this.locals.TryGetValue(expr,
                                        out int distance))
            {
                return this.Environment.GetAt(source,
                                              expr.name,
                                              distance);
            }

            return this.globals.Get(source,
                                    expr.name);
        }


        private double? Negate(Token token,
                               object? value)
        {
            if (value is double doubleValue)
            {
                return -doubleValue;
            }

            if (value is null)
            {
                return null;
            }

            throw new RuntimeError(token,
                                   "Operand must be a number.");
        }

        private double? Calculate(Token op,
                                  object? left,
                                  object? right,
                                  Func<double, double, double> calculation)
        {
            if (left is double doubleLeft
                && right is double doubleRight)
            {
                return calculation(doubleLeft,
                                   doubleRight);
            }

            if (left is null
                || right is null)
            {
                return null;
            }

            throw new RuntimeError(op,
                                   "Operands must be numbers.");
        }

        private bool Compare(Token op,
                             object? left,
                             object? right,
                             Func<double, double, bool> comparison)
        {
            if (left is double doubleLeft
                && right is double doubleRight)
            {
                return comparison(doubleLeft,
                                  doubleRight);
            }

            if (left is null
                || right is null)
            {
                return false;
            }

            throw new RuntimeError(op,
                                   "Operands must be numbers.");
        }

        private object? Evaluate(Expr expr,
                                 in ReadOnlySpan<char> source)
        {
            return expr.Accept(this,
                               source);
        }

        private bool IsEqual(object? left,
                             object? right)
        {
            if (left is null
                && right is null)
            {
                return true;
            }

            if (left is null)
            {
                return false;
            }

            return left.Equals(right);
        }

        private bool IsTruthy(object? value,
                              in ReadOnlySpan<char> source)
        {
            return value switch
            {
                null => false,
                false => false,
                _ => true
            };
        }

        private string Stringify(object? value)
        {
            if (value is double doubleValue)
            {
                string text = doubleValue.ToString();

                if (text.EndsWith(".0"))
                {
                    text = text.Substring(0,
                                          text.Length - 2);
                }

                return text;
            }

            return value?.ToString() ?? "nil";
        }

        #endregion
    }
}