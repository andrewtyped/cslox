using System.IO;
using System.Text;

using static System.Console;

using static lox.constants.ExitCodes;

namespace lox
{
    public class Lox
    {
        #region Class Methods

        /// <summary>
        /// Entry point for the lox interpreter. Runs code from a script file or from the command prompt as a REPL.
        /// </summary>
        /// <param name="args">0 or 1 string. If 0 args are passed, lox is run as a REPL, reading and executing
        /// one lox command at a time from the command prompt. The user may also pass a path to a file containing
        /// lox code.</param>
        /// <returns>
        /// If running as a REPL, the program does not return until the user exits the program forcefully with Ctrl + C
        /// or by killing the process.
        ///
        /// If interpreting a script file, cslox returns one of these codes:
        /// 0 - The program is interepreted successfully.
        /// 64 - Too many arguments passed to cslox.
        /// </returns>
        private static int Main(string[] args)
        {
            if (args.Length > 1)
            {
                WriteLine("Usage: cslox [script]");
                return EX_USAGE;
            }

            if (args.Length == 1)
            {
                return RunFile(args[0]);
            }

            return RunPrompt();
        }

        private static int Run(string code)
        {
            var scanner = new Scanner(code);
            var tokens = scanner.ScanTokens();

            for (int i = 0;
                 i < tokens.Count;
                 i++)
            {
                WriteLine(tokens[i]);
            }

            return EX_OK;
        }

        private static int RunFile(string filePath)
        {
            var bytes = File.ReadAllBytes(filePath);
            var returnCode = Run(Encoding.UTF8.GetString(bytes));

            if (hadError)
            {
                return EX_DATAERR;
            }

            return returnCode;
        }

        private static int RunPrompt()
        {
            for (;;)
            {
                Write("> ");
                var code = ReadLine();
                _ = Run(code);

                //Reset error. Lox errors shouldn't crash the user's REPL.
                hadError = false;
            }
        }

        #endregion

        #region Error Handling

        private static bool hadError;

        private static void Error(int line, string message)
        {
            Report(line,
                   "",
                   message);
        }

        private static void Report(int line, string where, string message)
        {
            WriteLine($"[line {line}] Error: {where}: {message}");
            hadError = true;
        }

        #endregion
    }
}