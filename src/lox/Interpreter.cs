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

            return expr.op.Type switch
            {
                EQUAL_EQUAL => this.IsEqual(left, right),
                BANG_EQUAL => !this.IsEqual(left, right),
                GREATER => (double?)left > (double?)right,
                LESS => (double?)left < (double?)right,
                GREATER_EQUAL => (double?)left >= (double?)right,
                LESS_EQUAL => (double?)left <= (double?)right,
                MINUS => (double?)left - (double?)right,
                SLASH => (double?)left / (double?)right,
                STAR => (double?)left * (double?)right,
                PLUS => this.VisitBinaryPlusOperands(left, right),
                _ => null
            };
        }

        private object? VisitBinaryPlusOperands(object? left,
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

            return null;
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