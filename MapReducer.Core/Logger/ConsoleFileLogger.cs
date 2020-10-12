using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MapReducer.Core.Logger
{
    public class ConsoleFileLogger : ILogger
    {
        string _filePath;
        object locker = new object();
        public ConsoleFileLogger(string filePath)
        {
            _filePath = filePath;
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        public void Log(string message)
        {
            string formatted = $"[{DateTime.Now}] - {message}";
            Console.WriteLine(formatted);
            lock (locker)
            {
                File.AppendAllLines(_filePath, new string[] { formatted });
            }
        }
    }
}
