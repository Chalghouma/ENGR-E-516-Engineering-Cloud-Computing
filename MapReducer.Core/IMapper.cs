using System;
using System.Collections.Generic;
using System.Text;

namespace MapReducer.Core
{
    interface IMapper<TInput, KOutput>
    {
        KOutput Map(TInput inputDataSet);
    }
}
