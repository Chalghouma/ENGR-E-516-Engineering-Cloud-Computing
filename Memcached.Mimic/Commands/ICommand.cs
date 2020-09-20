using System;
using System.Collections.Generic;
using System.Text;

namespace Memcached.Mimic.Commands
{
    public interface ICommand
    {
        string GetStringForEncoding();
    }
}
