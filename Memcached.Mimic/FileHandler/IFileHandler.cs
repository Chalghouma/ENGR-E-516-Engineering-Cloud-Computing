using System;
using System.Collections.Generic;
using System.Text;

namespace Memcached.Mimic.FileHandler
{
    public interface IFileHandler
    {
        string GetKeyValue(string keyName);
        bool SetKey(string keyName, string keyValue);
    }
}
