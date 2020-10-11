using MapReducer.Core.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MapReducer.Core
{
    public class WordCountMapper : IMapper<string, List<string>>
    {
        private string _endpoint;
        private ILogger _logger;
        public WordCountMapper(string endpoint, ILogger logger)
        {
            _endpoint = endpoint;
            _logger = logger;
        }
        public async Task<List<string>> Map(string inputDataSet)
        {
            Exception exceptionCaught = null;
            do
            {
                _logger.Log($"Mapping {inputDataSet}");
                try
                {

                    return await RestClient.PostJson<List<string>>(new
                    {
                        inputLine = inputDataSet
                    }, _endpoint);
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
