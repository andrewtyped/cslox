﻿using System;
using System.Collections.Generic;

using lox.constants;

using static lox.constants.FunctionType;

namespace lox
{
    /// <summary>
    /// Performs static analysis on the AST generated by <see cref="Parser"/>. 
    /// </summary>
    public class Resolver : Expr.IVisitor<object?>,
                            Stmt.IVisitor<Void>
    {
        #region Fields

        private readonly Interpreter interpreter;

        private readonly Stack<Dictionary<string, bool>> scopes;

        private FunctionType currentFunction = NONE;

        private ClassType currentClass = ClassType.NONE;

        internal readonly List<string> errors;

        #endregion

        #region Constructors

        public Resolver(Interpreter interpreter)
        {
            this.interpreter = interpreter ?? throw new ArgumentNullException(nameof(interpreter));
            this.scopes = new Stack<Dictionary<string, bool>>();
            this.errors = new List<string>();
        }

        #endregion

        #region Instance Methods

        public void Resolve(List<Stmt> stmts,
                            in ReadOnlySpan<char> source)
        {
            this.errors.Clear();

            for (int i = 0;
                 i < stmts.Count;
                 i++)
            {
                this.Resolve(stmts[i],
                             source);
            }
        }

        public object? VisitAssignExpr(Expr.Assign expr,
                                       in ReadOnlySpan<char> source)
        {
            this.Resolve(expr.value,
                         source);
            this.ResolveLocal(expr,
                              expr.name,
                              source);
            return default;
        }

        public object? VisitBinaryExpr(Expr.Binary expr,
                                       in ReadOnlySpan<char> source)
        {
            this.Resolve(expr.left,
                         source);
            this.Resolve(expr.right,
                         source);
            return default;
        }

        public Void VisitBlockStmt(Stmt.Block stmt,
                                   in ReadOnlySpan<char> source)
        {
            this.BeginScope();
            this.Resolve(stmt.statements,
                         source);
            this.EndScope();
            return default;
        }

        public object? VisitCallExpr(Expr.Call expr,
                                     in ReadOnlySpan<char> source)
        {
            this.Resolve(expr.callee,
                         source);

            for (int i = 0;
                 i < expr.arguments.Count;
                 i++)
            {
                this.Resolve(expr.arguments[i],
                             source);
            }

            return default;
        }

        public Void VisitClassStmt(Stmt.Class stmt,
                                   in ReadOnlySpan<char> source)
        {
            ClassType originalClass = this.currentClass;
            this.currentClass = ClassType.CLASS;

            this.Declare(stmt.name,
                         source);
            this.Define(stmt.name,
                        source);

            this.BeginScope();
            
            this.scopes.Peek()["this"] = true;

            for(int i = 0; i < stmt.methods.Count; i++)
            {
                string name = stmt.methods[i]
                                  .name.GetLexeme(source)
                                  .ToString();

                FunctionType declaration = name == "init"
                                               ? INITIALIZER
                                               : METHOD;

                this.ResolveFunction(stmt.methods[i],
                                     declaration,
                                     source);
            }

            this.EndScope();
            this.currentClass = originalClass;

            return default;
        }

        public Void VisitExpressionStmt(Stmt.Expression stmt,
                                        in ReadOnlySpan<char> source)
        {
            this.Resolve(stmt.expression,
                         source);
            return default;
        }

        public Void VisitFunctionStmt(Stmt.Function stmt,
                                      in ReadOnlySpan<char> source)
        {
            this.Declare(stmt.name,
                         source);
            this.Define(stmt.name,
                        source);

            this.ResolveFunction(stmt,
                                 FUNCTION,
                                 source);
            return default;
        }

        public object? VisitGetExpr(Expr.Get expr, 
                                    in ReadOnlySpan<char> source)
        {
            this.Resolve(expr.@object,
                         source);
            return default;
        }

        public object? VisitGroupingExpr(Expr.Grouping expr,
                                         in ReadOnlySpan<char> source)
        {
            this.Resolve(expr.expression,
                         source);
            return default;
        }

        public Void VisitIfStmt(Stmt.If stmt,
                                in ReadOnlySpan<char> source)
        {
            this.Resolve(stmt.condition,
                         source);
            this.Resolve(stmt.thenBranch,
                         source);
            if (stmt.elseBranch != null)
            {
                this.Resolve(stmt.elseBranch,
                             source);
            }

            return default;
        }

        public object? VisitLiteralExpr(Expr.Literal expr,
                                        in ReadOnlySpan<char> source)
        {
            return default;
        }

        public object? VisitLogicalExpr(Expr.Logical expr,
                                        in ReadOnlySpan<char> source)
        {
            this.Resolve(expr.left,
                         source);
            this.Resolve(expr.right,
                         source);
            return default;
        }

        public Void VisitPrintStmt(Stmt.Print stmt,
                                   in ReadOnlySpan<char> source)
        {
            this.Resolve(stmt.expression,
                         source);
            return default;
        }

        public Void VisitReturnStmt(Stmt.Return stmt,
                                    in ReadOnlySpan<char> source)
        {
            if (currentFunction == NONE)
            {
                this.Error(stmt.keyword.Line,
                           "Return statements are only allowed in functions and methods.");
            }

            if (stmt.value != null)
            {
                if(currentFunction == INITIALIZER)
                {
                    this.Error(stmt.keyword.Line,
                               "Cannot return a value from an initializer.");
                }

                this.Resolve(stmt.value,
                             source);
            }
            
            return default;
        }

        public object? VisitSetExpr(Expr.Set expr,
                                    in ReadOnlySpan<char> source)
        {
            this.Resolve(expr.value,
                         source);
            this.Resolve(expr.@object,
                         source);
            return default;
        }

        public object? VisitThisExpr(Expr.This expr,
                                     in ReadOnlySpan<char> source)
        {
            if(this.currentClass == ClassType.NONE)
            {
                this.Error(expr.keyword.Line,
                           "Cannot use 'this' outside of a class.");

                return default;
            }

            this.ResolveLocal(expr,
                              expr.keyword,
                              source);
            return default;
        }

        public object? VisitUnaryExpr(Expr.Unary expr,
                                      in ReadOnlySpan<char> source)
        {
            this.Resolve(expr.right,
                         source);
            return default;
        }

        public object? VisitVariableExpr(Expr.Variable expr,
                                         in ReadOnlySpan<char> source)
        {
            if (this.scopes.Count > 0)
            {
                var scope = this.scopes.Peek();
                var name = expr.name.GetLexeme(source)
                               .ToString();
                if (scope.TryGetValue(name,
                                      out bool defined))
                {
                    if (!defined)
                    {
                        this.Error(expr.name.Line,
                                   $"Cannot read local variable '{name}' in its own initializer");
                        return default;
                    }
                }
            }

            this.ResolveLocal(expr,
                              expr.name,
                              source);
            return default;
        }

        public Void VisitVarStmt(Stmt.Var stmt,
                                 in ReadOnlySpan<char> source)
        {
            this.Declare(stmt.name,
                         source);

            if (stmt.initializer != null)
            {
                this.Resolve(stmt.initializer,
                             source);
            }

            this.Define(stmt.name,
                        source);
            return default;
        }

        public Void VisitWhileStmt(Stmt.While stmt,
                                   in ReadOnlySpan<char> source)
        {
            this.Resolve(stmt.condition,
                         source);
            this.Resolve(stmt.statement,
                         source);
            return default;
        }

        private void BeginScope()
        {
            this.scopes.Push(new Dictionary<string, bool>());
        }

        private void Declare(Token name,
                             in ReadOnlySpan<char> source)
        {
            if (this.scopes.Count == 0)
            {
                return;
            }

            Dictionary<string, bool> scope = this.scopes.Peek();
            string lexeme = name.GetLexeme(source)
                                .ToString();

            if (scope.ContainsKey(lexeme))
            {
                this.Error(name.Line,
                           $"Variable '{lexeme}' is already declared in this scope.");
            }

            scope[lexeme] = false;
        }

        private void Define(Token name,
                            in ReadOnlySpan<char> source)
        {
            if (this.scopes.Count == 0)
            {
                return;
            }

            Dictionary<string, bool> scope = this.scopes.Peek();
            scope[name.GetLexeme(source)
                      .ToString()] = true;
        }

        private void EndScope()
        {
            this.scopes.Pop();
        }

        private void Resolve(Stmt stmt,
                             in ReadOnlySpan<char> source)
        {
            stmt.Accept(this,
                        source);
        }

        private void Resolve(Expr expr,
                             in ReadOnlySpan<char> source)
        {
            expr.Accept(this,
                        source);
        }

        private void ResolveFunction(Stmt.Function stmt,
                                     FunctionType functionType,
                                     in ReadOnlySpan<char> source)
        {
            FunctionType enclosingFunction = this.currentFunction;
            this.currentFunction = functionType;
            this.BeginScope();

            for (int i = 0;
                 i < stmt.parameters.Count;
                 i++)
            {
                this.Declare(stmt.parameters[i],
                             source);
                this.Define(stmt.parameters[i],
                            source);
            }

            this.Resolve(stmt.body,
                         source);

            this.EndScope();
            this.currentFunction = enclosingFunction;
        }

        private void ResolveLocal(Expr expr,
                                  Token exprName,
                                  in ReadOnlySpan<char> source)
        {
            var tempScopes = new Stack<Dictionary<string, bool>>();

            try
            {
                int scopesCount = this.scopes.Count;
                string name = exprName.GetLexeme(source)
                                      .ToString();

                for (int i = scopesCount - 1;
                     i >= 0;
                     i--)
                {
                    var tempScope = this.scopes.Pop();
                    tempScopes.Push(tempScope);

                    if (tempScope.ContainsKey(name))
                    {
                        this.interpreter.Resolve(expr,
                                                 scopesCount - 1 - i);
                        return;
                    }
                }
            }
            finally
            {
                while (tempScopes.TryPop(out Dictionary<string, bool>? tempScope))
                {
                    this.scopes.Push(tempScope);
                }
            }
        }

        #endregion

        #region Error Handling

        private void Error(int line,
                           string message)
        {
            Lox.Error(line,
                      message);
            this.errors.Add(message);
        }

        #endregion
    }
}