using MapReducer.Core.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MapReducer.Core
{
    public class WordCountMapper : IMapper<string, string, int>
    {
        private string _azureFunctionUrl;
        private ILogger _logger;
        public WordCountMapper(string azureFunctionUrl, ILogger logger)
        {
            _azureFunctionUrl = azureFunctionUrl;
            _logger = logger;
        }
        public async Task<Dictionary<string, int>> Map(string inputDataSet)
        {
            Exception exceptionCaught = null;
            do
            {
                _logger.Log($"Mapping {inputDataSet}");
                try
                {

                    return await RestClient.PostJson<Dictionary<string, int>>(new
                    {
                        inputLine = inputDataSet
                    }, "http://localhost:7071/api/WordCountMapper");
                }
                catch (Exception exp)
                {
                    exceptionCaught = exp;
                    _logger.Log($"Exception Caught while Mapping {inputDataSet}. Message: {exp.Message}");
                }
            }
            while (exceptionCaught != null);

            return null;
        }
    }
}
