using MapReducer.Core.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapReducer.Core
{
    public class WordCountReducer : IReducer<string, string>
    {
        private string _azureFunctionsUrl;
        private ILogger _logger;
        public WordCountReducer(string url, ILogger logger)
        {
            _azureFunctionsUrl = url;
            _logger = logger;
        }
        class Result
        {
            public string key;
        }
        public async Task<string> Reduce(string inputKey)
        {

            Exception exceptionCaught = null;
            do
            {
                _logger.Log($"Reducing inputKey:{inputKey}'s values from {_azureFunctionsUrl}");
                try
                {
                    var result = (await RestClient.PostJson<Result>(new { key = inputKey }, _azureFunctionsUrl));
                    return result.key;
                }
                catch (Exception exp)
                {
                    exceptionCaught = exp;
                    _logger.Log($"Exception Caught while reducing {inputKey}. Message: {exp.Message}");
                }
            }
            while (exceptionCaught != null);

            return null;
        }
    }
}
