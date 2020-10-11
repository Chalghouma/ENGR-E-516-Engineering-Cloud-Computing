using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MapReducer.Core
{
    interface IMapper<TInput, TOutput>
    {
        Task<TOutput> Map(TInput inputDataSet);
    }
}
