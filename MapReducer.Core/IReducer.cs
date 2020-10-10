using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MapReducer.Core
{
    interface IReducer<TKey, TValue>
    {
        Task<KeyValuePair<TKey, TValue>> Reduce(List<KeyValuePair<TKey, TValue>> input);
    }
}
