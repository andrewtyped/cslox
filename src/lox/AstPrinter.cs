using System;
using System.Text;

namespace lox
{
    /// <summary>
    /// Expression visitor capable of rendering a lox AST as a string which illustrates the tree's structure.
    /// </summary>
    public class AstPrinter : Expr.IVisitor<string>
    {
        #region Instance Methods

        public string Print(Expr expr,
                            in ReadOnlySpan<char> source)
        {
            return expr.Accept(this,
                               source);
        }

        public string VisitBinaryExpr(Expr.Binary expr,
                                      in ReadOnlySpan<char> source)
        {
            return this.Parenthesize(expr.op.GetLexeme(source),
                                     source,
                                     expr.left,
                                     expr.right);
        }

        public string VisitGroupingExpr(Expr.Grouping expr,
                                        in ReadOnlySpan<char> source)
        {
            return this.Parenthesize("group",
                                     source,
                                     expr.expression);
        }

        public string VisitLiteralExpr(Expr.Literal expr,
                                       in ReadOnlySpan<char> source)
        {
            return expr.value?.ToString() ?? "nil";
        }

        public string VisitUnaryExpr(Expr.Unary expr,
                                     in ReadOnlySpan<char> source)
        {
            return this.Parenthesize(expr.op.GetLexeme(source),
                                     source,
                                     expr.right);
        }

        private string Parenthesize(in ReadOnlySpan<char> name,
                                    in ReadOnlySpan<char> source,
                                    params Expr[] exprs)
        {
            var sb = new StringBuilder();

            sb.Append('(')
              .Append(name);

            for (int i = 0;
                 i < exprs.Length;
                 i++)
            {
                sb.Append(' ')
                  .Append(exprs[i]
                              .Accept(this,
                                      source));
            }

            sb.Append(')');
            return sb.ToString();
        }

        #endregion
    }
}