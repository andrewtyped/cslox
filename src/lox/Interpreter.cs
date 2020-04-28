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

            return expr.op.Type switch
            {
                EQUAL_EQUAL => this.IsEqual(left, right),
                BANG_EQUAL => !this.IsEqual(left, right),
                GREATER => (double?)left > (double?)right,
                LESS => (double?)left < (double?)right,
                GREATER_EQUAL => (double?)left >= (double?)right,
                LESS_EQUAL => (double?)left <= (double?)right,
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
                MINUS => -((double?)right),
                BANG => !this.IsTruthy(right,
                                       source),
                _ => null
            };
        }

        #endregion

        #region Utilities

        private double? Calculate(Token op,
                                  object? left,
                                  object? right,
                                  Func<double, double, double> calculation)
        {
            if(left is null || right is null)
            {
                return null;
            }

            if(left is double doubleLeft && right is double doubleRight)
            {
                return calculation(doubleLeft,
                                   doubleRight);
            }

            throw new RuntimeError(op,
                                   "Operand must be a number.");
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