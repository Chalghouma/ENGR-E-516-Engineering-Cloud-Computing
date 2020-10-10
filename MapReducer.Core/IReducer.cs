using System;
using System.Collections.Generic;
using System.Text;

namespace MapReducer.Core
{
    interface IReducer<TKey, TValue>
    {
        KeyValuePair<TKey, TValue> Reduce(List<KeyValuePair<TKey, TValue>> input);
    }
}
