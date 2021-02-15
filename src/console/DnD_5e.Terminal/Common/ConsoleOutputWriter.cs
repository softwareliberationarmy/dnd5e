using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD_5e.Terminal.Common
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
