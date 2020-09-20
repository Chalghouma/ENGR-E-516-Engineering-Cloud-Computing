using System;
using System.Collections.Generic;
using System.Text;

namespace Memcached.Mimic.Commands
{
    public class SetCommand : ICommand
    {
        public string Key { get; private set; }
        public int ValueSize { get; private set; }
        public string Value { get; private set; }
        public SetCommand(string key, int valueSize, string value)
        {
            Key = key; ValueSize = valueSize; Value = value;
        }

        public string GetStringForEncoding()
        {
            return $"set {Key} {ValueSize} {Value}";
        }
    }
}
