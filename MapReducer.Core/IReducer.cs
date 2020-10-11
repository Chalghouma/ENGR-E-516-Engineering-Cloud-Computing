using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MapReducer.Core
{
    interface IReducer<TInputKey, TOutput>
    {
        Task<TOutput> Reduce(TInputKey inputKey);
    }
}
