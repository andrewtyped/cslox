using System;

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

            return 0;
        }
    }
}
