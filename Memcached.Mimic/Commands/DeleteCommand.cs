using System;
using System.Collections.Generic;
using System.Text;

namespace Memcached.Mimic.Commands
{
    public class DeleteCommand : ICommand
    {
        public string Key { get; private set; }
        public DeleteCommand(string key)
        {
            Key = key;
        }
        public string GetStringForEncoding()
        {
            return $"delete {Key}";
        }
    }
}
