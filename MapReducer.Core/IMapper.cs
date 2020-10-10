using System;
using System.Collections.Generic;
using System.Text;

namespace MapReducer.Core
{
    interface IMapper<TInput, TOutputKey, TOutputValue>
    {
        Dictionary<TOutputKey, TOutputValue> Map(TInput inputDataSet);
    }
}
