using System;

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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        #endregion

        #region Utilities

        private object? Evaluate(Expr expr,
                                 in ReadOnlySpan<char> source)
        {
            return expr.Accept(this,
                               source);
        }

        #endregion
    }
}