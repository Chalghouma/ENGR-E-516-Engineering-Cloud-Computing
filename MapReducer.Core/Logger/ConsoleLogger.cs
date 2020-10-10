using System;
using System.Collections.Generic;
using System.Text;

namespace MapReducer.Core.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"[{DateTime.Now}] - {message}");
        }
    }
}
