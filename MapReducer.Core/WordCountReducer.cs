using MapReducer.Core.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapReducer.Core
{
    public class WordCountReducer : IReducer<string, int>
    {
        private string _azureFunctionsUrl;
        private ILogger _logger;
        public WordCountReducer(string url, ILogger logger)
        {
            _azureFunctionsUrl = url;
            _logger = logger;
        }
        public async Task<KeyValuePair<string, int>> Reduce(List<KeyValuePair<string, int>> input)
        {

            Exception exceptionCaught = null;
            do
            {
                _logger.Log($"Mapping {JsonConvert.SerializeObject(input)}");
                try
                {
                    return await RestClient.PostJson<KeyValuePair<string, int>>(input, _azureFunctionsUrl);
                }
                catch (Exception exp)
                {
                    exceptionCaught = exp;
                    _logger.Log($"Exception Caught while Mapping {JsonConvert.SerializeObject(input)}. Message: {exp.Message}");
                }
            }
            while (exceptionCaught != null);
            
            return new KeyValuePair<string, int>();
        }
    }
}
