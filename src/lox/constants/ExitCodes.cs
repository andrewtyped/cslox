﻿namespace lox.constants
{
    /// <summary>
    /// Program exit codes defined by sysexits.h from the FreeBSD manual:
    /// https://www.freebsd.org/cgi/man.cgi?query=sysexits&apropos=0&sektion=0&manpath=FreeBSD+4.3-RELEASE&format=html
    /// </summary>
    public static class ExitCodes
    {
        /// <summary>
        /// The command executed successfully.
        /// </summary>
        public const int EX_OK = 0;

        /// <summary>
        /// The command was used incorrectly, e.g., with the wrong number of arguments, a bad flag, a bad syntax in a parameter, etc.
        /// </summary>
        public const int EX_USAGE = 64;
    }
}