using System;

using static lox.constants.TokenType;

namespace lox
{
    /// <summary>
    /// Evaluates a Lox AST and produces its value.
    /// </summary>
    public class Interpreter : Expr.IVisitor<object?>
    {
        #region Instance Methods
        #endregion

        #region Expression visitors

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
                EQUAL_EQUAL => this.IsEqual(left, right),
                BANG_EQUAL => !this.IsEqual(left, right),
                GREATER => CompareLocal((a, b) => a > b),
                LESS => CompareLocal((a, b) => a < b),
                GREATER_EQUAL => CompareLocal((a, b) => a >= b),
                LESS_EQUAL => CompareLocal((a, b) => a <= b),
                MINUS => CalculateLocal((a, b) => a - b),
                SLASH => CalculateLocal((a, b) => a / b),
                STAR => CalculateLocal((a, b) => a * b),
                PLUS => this.VisitBinaryPlusOperands(expr.op, left, right),
                _ => null
            };
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

        private double? Negate(Token token, object? value)
        {
            if(value is double doubleValue)
            {
                return -doubleValue;
            }

            if(value is null)
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
            if(left is double doubleLeft && right is double doubleRight)
            {
                return calculation(doubleLeft,
                                   doubleRight);
            }

            if(left is null || right is null)
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

        private bool IsEqual(object? left, object? right)
        {
            if(left is null && right is null)
            {
                return true;
            }

            if(left is null)
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

        #endregion
    }
}