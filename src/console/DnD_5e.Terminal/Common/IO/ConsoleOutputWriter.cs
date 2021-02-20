using System;

namespace DnD_5e.Terminal.Common.IO
{
    public class ConsoleOutputWriter : IOutputWriter
    {
        public void WriteLine(string line)
        {
            Console.WriteLine("{0}{1}", Prefix, line);
        }

        public string Prefix { get; } = "DnD >> ";
    }
}
