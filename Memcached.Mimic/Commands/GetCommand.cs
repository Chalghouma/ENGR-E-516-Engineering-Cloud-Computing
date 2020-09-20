using System;
using System.Collections.Generic;
using System.Text;

namespace Memcached.Mimic.Commands
{
    public class GetCommand : ICommand
    {
        public string Key { get; private set; }
        public GetCommand(string key)
        {
            Key = key;
        }

        public string GetStringForEncoding()
        {
            return $"get {Key}";
        }
    }
}
