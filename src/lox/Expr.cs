﻿/*
** This file is generated by the 'tool' project. Do not modify by hand.
*/

using System;
using System.Collections.Generic;

namespace lox
{
    public abstract class Expr
    {
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

        }


        public class Grouping : Expr
        {
            public Grouping( Expr expression )
            {
                this.expression = expression;
            }

            public readonly Expr expression;

        }


        public class Literal : Expr
        {
            public Literal( Object value )
            {
                this.value = value;
            }

            public readonly Object value;

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

        }


    }
}