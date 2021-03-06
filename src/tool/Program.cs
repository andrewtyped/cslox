﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace tool
{
    class Program
    {
        static int Main(string[] args)
        {
            if(args.Length != 1)
            {
                Console.Error.WriteLine("Usage: tool.exe <output directory>");
                return 1;
            }

            var outputDir = args[0];

            DefineAst(outputDir,
                      "Expr",
                      new List<string>
                      {
                          "Assign   : Token name, Expr value",
                          "Binary   : Expr left, Token op, Expr right",
                          "Call     : Expr callee, Token paren, List<Expr> arguments",
                          "Get      : Token name, Expr @object",
                          "Grouping : Expr expression",
                          "Literal  : object? value",
                          "Logical  : Expr left, Token op, Expr right",
                          "Set      : Expr @object, Token name, Expr value",
                          "Super    : Token keyword, Token method",
                          "This     : Token keyword",
                          "Unary    : Token op, Expr right",
                          "Variable : Token name",
                      });

            DefineAst(outputDir,
                      "Stmt",
                      new List<string>
                      {
                          "Block      : List<Stmt> statements",
                          "Class      : Token name, Expr.Variable? superclass, List<Stmt.Function> methods",
                          "Expression : Expr expression",
                          "Function   : Token name, List<Token> parameters, List<Stmt> body",
                          "If         : Expr condition, Stmt thenBranch, Stmt? elseBranch",
                          "Print      : Expr expression",
                          "Return     : Token keyword, Expr? value",
                          "Var        : Token name, Expr? initializer",
                          "While      : Expr condition, Stmt statement"
                      });

            return 0;
        }

        private static void DefineAst(string outputDir, string baseName, List<string> types)
        {
            string path = Path.Combine(outputDir,
                                       $"{baseName}.cs");

            using var streamWriter = new StreamWriter(path,
                                                      append: false,
                                                      Encoding.UTF8);

            string indent = "";
            void WriteLine(string line) => streamWriter.WriteLine(indent + line);
            void PushIndent() => indent += "    ";

            void PopIndent() =>
                indent = indent.Remove(0,
                                       4);
            void NewLine() => streamWriter.WriteLine();

            void DefineType(string className, string fields)
            {
                WriteLine($"public class {className} : {baseName}");
                WriteLine("{");
                PushIndent();

                //Constructor
                WriteLine($"public {className}( {fields} )");
                WriteLine("{");
                PushIndent();

                var fieldList = fields.Split(", ");
                foreach(var field in fieldList)
                {
                    string name = field.Split(" ")[1];
                    WriteLine($"this.{name} = {name};");
                }

                PopIndent();
                WriteLine("}");
                //End constructor

                NewLine();

                //Fields
                foreach(var field in fieldList)
                {
                    string name = field.Split(" ")[1];
                    WriteLine($"public readonly {field};");
                    NewLine();
                }
                //End Fields

                //Visitor Pattern
                WriteLine("public override R Accept<R>(IVisitor<R> visitor, in ReadOnlySpan<char> source)");
                WriteLine("{");
                PushIndent();
                WriteLine($"return visitor.Visit{className}{baseName}(this, source);");
                PopIndent();
                WriteLine("}");
                //End Visitor

                PopIndent();
                WriteLine("}");
                NewLine();
                NewLine();
            }

            void DefineVisitor()
            {
                WriteLine("public interface IVisitor<R>");
                WriteLine("{");
                PushIndent();

                foreach(string type in types)
                {
                    
                    string typeName = type.Split(":")[0]
                                          .Trim();
                    WriteLine($"R Visit{typeName}{baseName}({typeName} {baseName.ToLower()}, in ReadOnlySpan<char> source);");
                }

                PopIndent();
                WriteLine("}");
                NewLine();
            }

            WriteLine("/*");
            WriteLine("** This file is generated by the 'tool' project. Do not modify by hand.");
            WriteLine("*/");
            NewLine();
            WriteLine("using System;");
            WriteLine("using System.Collections.Generic;");
            NewLine();
            WriteLine("namespace lox");
            WriteLine("{");
            PushIndent();
            WriteLine($"public abstract class {baseName}");
            WriteLine("{");
            PushIndent();

            DefineVisitor();

            foreach(string type in types)
            {
                string className = type.Split(":")[0]
                                       .Trim();
                string fields = type.Split(":")[1]
                                    .Trim();

                DefineType(className,
                           fields);
            }

            WriteLine($"public abstract R Accept<R>(IVisitor<R> visitor, in ReadOnlySpan<char> source);");

            PopIndent();
            WriteLine("}");
            PopIndent();
            WriteLine("}");

        }
    }
}
