using System;
using System.Collections.Generic;
using System.Text;

namespace Memcached.Mimic.FileHandler
{
    public interface IFileHandler
    {
        bool GetKeyValue(string keyName,out string keyValue);
        bool SetKey(string keyName, string keyValue);
    }
}
