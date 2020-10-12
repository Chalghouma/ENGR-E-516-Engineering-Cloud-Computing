using MapReducer.Core.Logger;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MapReducer.Core.InvertedIndex
{
    public class DocumentData
    {
        public int index;
        public string content;
    }
    public class InvertedIndexMapper : IMapper<DocumentData, List<string>>
    {
        private string _endpoint;
        private ILogger _logger;
        public InvertedIndexMapper(string endpoint, ILogger logger)
        {
            _endpoint = endpoint;
            _logger = logger;
        }
        public async Task<List<string>> Map(DocumentData inputDataSet)
        {
            Exception exceptionCaught = null;
            do
            {
                _logger.Log($"Mapping document of index {inputDataSet.index}");
                try
                {
                    return await RestClient.PostJson<List<string>>(new
                    {
                        index=inputDataSet.index,
                        content=inputDataSet.content
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
