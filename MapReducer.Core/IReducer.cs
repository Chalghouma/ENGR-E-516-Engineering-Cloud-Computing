using System;
using System.Collections.Generic;
using System.Text;

namespace MapReducer.Core
{
    interface IReducer<TInput,KOutput>
    {
        KOutput Reduce(TInput input);
    }
}
