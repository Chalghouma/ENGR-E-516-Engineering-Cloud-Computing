using MapReducer.Core.Logger;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MapReducer.Core.InvertedIndex
{
    public class WordOccurence
    {
        public int index;
        public int occurence;
    }
    public class InvertedIndexReducer : IReducer<string, string>
    {
        private string _endpoint;
        private ILogger _logger;
        public InvertedIndexReducer(string endpoint, ILogger logger)
        {
            _endpoint = endpoint;
            _logger = logger;
        }
        public async Task<string> Reduce(string wordKey)
        {
            Exception exceptionCaught = null;
            do
            {
                _logger.Log($"Reducing inputKey:{wordKey}'s values from {_endpoint}");
                try
                {
                    return  (await RestClient.PostJson<StoredItem<List<WordOccurence>>>(new { key = wordKey }, _endpoint)).key;
                }
                catch (Exception exp)
                {
                    exceptionCaught = exp;
                    _logger.Log($"Exception Caught while reducing {wordKey}. Message: {exp.Message}");
                }
            }
            while (exceptionCaught != null);

            return null;
        }
    }
}
