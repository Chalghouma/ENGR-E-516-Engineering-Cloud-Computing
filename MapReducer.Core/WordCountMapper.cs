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

                    var client = new HttpClient();
                    var stringContent = new StringContent(JsonConvert.SerializeObject(new
                    {
                        inputLine = inputDataSet
                    }), Encoding.UTF8, "application/json");
                    var requestResult = await client.PostAsync(_azureFunctionUrl, stringContent);
                    var serializedResponse = await requestResult.Content.ReadAsStringAsync();
                    var deserialized = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializedResponse);
                    return deserialized;
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
