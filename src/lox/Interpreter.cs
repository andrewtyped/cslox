using System;

namespace lox
{
    /// <summary>
    /// Evaluates a Lox AST and produces its value.
    /// </summary>
    public class Interpreter : Expr.IVisitor<object?>
    {
        #region Instance Methods

        public object? VisitBinaryExpr(Expr.Binary expr,
                                      in ReadOnlySpan<char> source)
        {
            throw new NotImplementedException();
        }

        public object? VisitGroupingExpr(Expr.Grouping expr,
                                        in ReadOnlySpan<char> source)
        {
            throw new NotImplementedException();
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
    }
}