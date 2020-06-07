using System;
using System.Collections.Generic;
using System.Text;

namespace lox
{
    /// <summary>
    /// Expression visitor capable of rendering a lox AST as a string which illustrates the tree's structure.
    /// </summary>
    public class AstPrinter : Expr.IVisitor<string>, Stmt.IVisitor<string>
    {
        #region Instance Methods

        public string Print(List<Stmt> stmts,
                            in ReadOnlySpan<char> source)
        {
            var sb = new StringBuilder();

            for(int i = 0; i < stmts.Count; i++)
            {
                sb.AppendLine(stmts[i]
                                  .Accept(this,
                                          source));
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public string Print(Expr expr,
                            in ReadOnlySpan<char> source)
        {
            return expr.Accept(this,
                               source);
        }

        #endregion

        #region Statement visitors

        public string VisitClassStmt(Stmt.Class stmt,
                                     in ReadOnlySpan<char> source)
        {
            throw new NotImplementedException();
        }

        public string VisitFunctionStmt(Stmt.Function stmt,
                                        in ReadOnlySpan<char> source)
        {
            throw new NotImplementedException();
        }

        public string VisitIfStmt(Stmt.If stmt,
                                  in ReadOnlySpan<char> source)
        {
            throw new NotImplementedException();
        }

        public string VisitBlockStmt(Stmt.Block stmt,
                                     in ReadOnlySpan<char> source)
        {
            return this.Parenthesize("block",
                                     source,
                                     stmt.statements.ToArray());
        }

        public string VisitExpressionStmt(Stmt.Expression stmt,
                                          in ReadOnlySpan<char> source)
        {
            return stmt.expression.Accept(this,
                                          source);
        }

        public string VisitPrintStmt(Stmt.Print stmt,
                                     in ReadOnlySpan<char> source)
        {
            return this.Parenthesize("print",
                                     source,
                                     stmt.expression);
        }

        public string VisitReturnStmt(Stmt.Return stmt,
                                      in ReadOnlySpan<char> source)
        {
            if(stmt.value is null)
            {
                return this.Parenthesize("return",
                                         source,
                                         Array.Empty<Expr>());
            }

            return this.Parenthesize("return",
                                     source,
                                     stmt.value);
        }

        public string VisitVarStmt(Stmt.Var stmt, in ReadOnlySpan<char> source)
        {
            throw new NotImplementedException();
        }

        public string VisitWhileStmt(Stmt.While stmt, in ReadOnlySpan<char> source)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Expression visitors

        public string VisitAssignExpr(Expr.Assign expr,
                                      in ReadOnlySpan<char> source)
        {
            return this.Parenthesize($"assign {expr.name.GetLexeme(source).ToString()}",
                                     source,
                                     expr.value);
        }

        public string VisitCallExpr(Expr.Call expr,
                                    in ReadOnlySpan<char> source)
        {
            throw new NotImplementedException();
        }

        public string VisitVariableExpr(Expr.Variable expr,
                                        in ReadOnlySpan<char> source)
        {
            throw new NotImplementedException();
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

        public string VisitLogicalExpr(Expr.Logical expr,
                                       in ReadOnlySpan<char> source)
        {
            return this.Parenthesize(expr.op.GetLexeme(source),
                                     source,
                                     expr.left,
                                     expr.right);
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

        private string Parenthesize(in ReadOnlySpan<char> name,
                                    in ReadOnlySpan<char> source,
                                    params Stmt[] stmts)
        {
            var sb = new StringBuilder();

            sb.Append('(')
              .Append(name);

            for (int i = 0;
                 i < stmts.Length;
                 i++)
            {
                sb.Append(' ')
                  .Append(stmts[i]
                              .Accept(this,
                                      source))
                  .AppendLine(";");
            }

            sb.Append(')');
            return sb.ToString();
        }

        #endregion
    }
}