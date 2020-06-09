﻿/*
** This file is generated by the 'tool' project. Do not modify by hand.
*/

using System;
using System.Collections.Generic;

namespace lox
{
    public abstract class Expr
    {
        public interface IVisitor<R>
        {
            R VisitAssignExpr(Assign expr, in ReadOnlySpan<char> source);
            R VisitBinaryExpr(Binary expr, in ReadOnlySpan<char> source);
            R VisitCallExpr(Call expr, in ReadOnlySpan<char> source);
            R VisitGetExpr(Get expr, in ReadOnlySpan<char> source);
            R VisitGroupingExpr(Grouping expr, in ReadOnlySpan<char> source);
            R VisitLiteralExpr(Literal expr, in ReadOnlySpan<char> source);
            R VisitLogicalExpr(Logical expr, in ReadOnlySpan<char> source);
            R VisitSetExpr(Set expr, in ReadOnlySpan<char> source);
            R VisitThisExpr(This expr, in ReadOnlySpan<char> source);
            R VisitUnaryExpr(Unary expr, in ReadOnlySpan<char> source);
            R VisitVariableExpr(Variable expr, in ReadOnlySpan<char> source);
        }

        public class Assign : Expr
        {
            public Assign( Token name, Expr value )
            {
                this.name = name;
                this.value = value;
            }

            public readonly Token name;

            public readonly Expr value;

            public override R Accept<R>(IVisitor<R> visitor, in ReadOnlySpan<char> source)
            {
                return visitor.VisitAssignExpr(this, source);
            }
        }


        public class Binary : Expr
        {
            public Binary( Expr left, Token op, Expr right )
            {
                this.left = left;
                this.op = op;
                this.right = right;
            }

            public readonly Expr left;

            public readonly Token op;

            public readonly Expr right;

            public override R Accept<R>(IVisitor<R> visitor, in ReadOnlySpan<char> source)
            {
                return visitor.VisitBinaryExpr(this, source);
            }
        }


        public class Call : Expr
        {
            public Call( Expr callee, Token paren, List<Expr> arguments )
            {
                this.callee = callee;
                this.paren = paren;
                this.arguments = arguments;
            }

            public readonly Expr callee;

            public readonly Token paren;

            public readonly List<Expr> arguments;

            public override R Accept<R>(IVisitor<R> visitor, in ReadOnlySpan<char> source)
            {
                return visitor.VisitCallExpr(this, source);
            }
        }


        public class Get : Expr
        {
            public Get( Token name, Expr @object )
            {
                this.name = name;
                this.@object = @object;
            }

            public readonly Token name;

            public readonly Expr @object;

            public override R Accept<R>(IVisitor<R> visitor, in ReadOnlySpan<char> source)
            {
                return visitor.VisitGetExpr(this, source);
            }
        }


        public class Grouping : Expr
        {
            public Grouping( Expr expression )
            {
                this.expression = expression;
            }

            public readonly Expr expression;

            public override R Accept<R>(IVisitor<R> visitor, in ReadOnlySpan<char> source)
            {
                return visitor.VisitGroupingExpr(this, source);
            }
        }


        public class Literal : Expr
        {
            public Literal( object? value )
            {
                this.value = value;
            }

            public readonly object? value;

            public override R Accept<R>(IVisitor<R> visitor, in ReadOnlySpan<char> source)
            {
                return visitor.VisitLiteralExpr(this, source);
            }
        }


        public class Logical : Expr
        {
            public Logical( Expr left, Token op, Expr right )
            {
                this.left = left;
                this.op = op;
                this.right = right;
            }

            public readonly Expr left;

            public readonly Token op;

            public readonly Expr right;

            public override R Accept<R>(IVisitor<R> visitor, in ReadOnlySpan<char> source)
            {
                return visitor.VisitLogicalExpr(this, source);
            }
        }


        public class Set : Expr
        {
            public Set( Expr @object, Token name, Expr value )
            {
                this.@object = @object;
                this.name = name;
                this.value = value;
            }

            public readonly Expr @object;

            public readonly Token name;

            public readonly Expr value;

            public override R Accept<R>(IVisitor<R> visitor, in ReadOnlySpan<char> source)
            {
                return visitor.VisitSetExpr(this, source);
            }
        }


        public class This : Expr
        {
            public This( Token keyword )
            {
                this.keyword = keyword;
            }

            public readonly Token keyword;

            public override R Accept<R>(IVisitor<R> visitor, in ReadOnlySpan<char> source)
            {
                return visitor.VisitThisExpr(this, source);
            }
        }


        public class Unary : Expr
        {
            public Unary( Token op, Expr right )
            {
                this.op = op;
                this.right = right;
            }

            public readonly Token op;

            public readonly Expr right;

            public override R Accept<R>(IVisitor<R> visitor, in ReadOnlySpan<char> source)
            {
                return visitor.VisitUnaryExpr(this, source);
            }
        }


        public class Variable : Expr
        {
            public Variable( Token name )
            {
                this.name = name;
            }

            public readonly Token name;

            public override R Accept<R>(IVisitor<R> visitor, in ReadOnlySpan<char> source)
            {
                return visitor.VisitVariableExpr(this, source);
            }
        }


        public abstract R Accept<R>(IVisitor<R> visitor, in ReadOnlySpan<char> source);
    }
}
