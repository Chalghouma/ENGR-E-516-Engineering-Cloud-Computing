using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapReducer.Core
{
    public class WordCountReducer : IReducer<string, int>
    {
        public async Task<KeyValuePair<string, int>> Reduce(List<KeyValuePair<string, int>> input)
        {
            int aggregated = input.Aggregate(0, (result, item) => result + item.Value);
            return new KeyValuePair<string, int>(input[0].Key, aggregated);
        }
    }
}
