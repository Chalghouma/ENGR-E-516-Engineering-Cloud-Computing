using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Memcached.Mimic.FileHandler
{
    public class TextFileHandler : IFileHandler
    {
        const string DefaultFilePath = "Data.txt";
        string _filePath;
        object fileLock = new object();
        public TextFileHandler(string filePath = DefaultFilePath)
        {
            _filePath = filePath;
            EnsureFileExists();
        }
        void EnsureFileExists()
        {
            lock (fileLock)
            {
                if (!File.Exists(_filePath))
                {
                    using (var fStream = File.Create(_filePath))
                    {
                    }
                }
            }
        }
        public bool GetKeyValue(string keyName, out string keyValue)
        {
            lock (fileLock)
            {
                string[] lines = File.ReadAllLines(_filePath);
                foreach (var line in lines)
                {
                    string[] splitted = line.Split(' ');
                    if (splitted[0] == keyName)
                    {
                        keyValue = splitted[1];
                        return true;
                    }
                }
            }
            keyValue = "";
            return false;
        }

        public bool SetKey(string keyName, string keyValue)
        {
            lock (fileLock)
            {
                bool found = false;
                StringBuilder builder = new StringBuilder();
                foreach (var line in File.ReadAllLines(_filePath))
                {
                    string[] splitted = line.Split(' ');
                    if (splitted[0] == keyName)
                    {
                        found = true;
                        string newLine = keyName + " " + keyValue;
                        builder.AppendLine(newLine);
                    }
                    else
                    {
                        builder.AppendLine(line);
                    }

                }
                if (!found)
                    builder.AppendLine(keyName + " " + keyValue);
                File.WriteAllText(_filePath, builder.ToString());
            }
            return true;
        }

        public bool DeleteKey(string keyName)
        {
            bool found = false;
            lock (fileLock)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var line in File.ReadAllLines(_filePath))
                {
                    string[] splitted = line.Split(' ');
                    if (splitted[0] == keyName)
                    {
                        found = true;
                    }
                    else
                    {
                        builder.AppendLine(line);
                    }

                }

                File.WriteAllText(_filePath, builder.ToString());
            }
            return found;
        }
    }
}
