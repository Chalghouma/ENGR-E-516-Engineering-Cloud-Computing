using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MapReducer.Core
{
    interface IMapper<TInput, TOutputKey, TOutputValue>
    {
        Task<Dictionary<TOutputKey, TOutputValue>> Map(TInput inputDataSet);
    }
}
