using MapReducer.Core.Logger;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapReducer.Core.InvertedIndex
{
    public class InvertedIndexSplitter
    {
        public static async Task ProcessDocuments(string[] filePaths, string azureFunctionsBaseUrl, ILogger logger)
        {
            await DeleteCache(logger, azureFunctionsBaseUrl);
            logger.Log($"Processing {filePaths.Length} Documents");
            List<string> fileContents = filePaths.Select(filePath => File.ReadAllText(filePath)).ToList();
            var asyncMapTasks = new List<Task<List<string>>>();
            for (int i = 0; i < fileContents.Count; i++)
            {
                var data = new DocumentData { index = i, content = fileContents[i] };
                asyncMapTasks.Add(Task.Run(() => new InvertedIndexMapper($"{azureFunctionsBaseUrl}/InvertedIndexMapper", logger)
                .Map(data)));
            }
            var mapTasksOutputs = await Task.WhenAll(asyncMapTasks);
            List<string> keys = new List<string>();
            foreach (var mapTaskOutput in mapTasksOutputs)
            {
                foreach (var key in mapTaskOutput)
                {
                    keys.Add(key);
                }
            }
            keys = keys.Distinct().ToList();


            var asyncReduceTasks = keys.Select(key => Task.Run(() => new InvertedIndexReducer($"{azureFunctionsBaseUrl}/InvertedIndexReducer", logger).Reduce(key))
            ).ToList();
            var reduceTasksOutputs = await Task.WhenAll(asyncReduceTasks);
            logger.Log("All Reducers have ended");
            logger.Log("Starting to get end-results from KeyStore");
            foreach (var reduceTaskoutput in reduceTasksOutputs)
            {
                string url = $"{azureFunctionsBaseUrl}/GetValueFunction";
                var resultValue = await RestClient.PostJson<StoredItem<List<List<WordOccurence>>>>(new { key = reduceTaskoutput }, url);
                logger.Log($"{resultValue.key.Replace("_REDUCER_OUTPUT","")} has Value = {JsonConvert.SerializeObject(resultValue.value[0])}");
                //Console.WriteLine(JsonConvert.SerializeObject(resultValue));
            }
        }
        private async static Task DeleteCache(ILogger logger, string baseUrl)
        {
            string endpoint = baseUrl + "/ClearContainerFunction";
            Exception exp = null;
            do
            {
                try
                {

                    logger.Log($"Attempting to ClearCache from {endpoint}");
                    await RestClient.PostJson<object>(new { }, endpoint);
                    exp = null;
                }
                catch (Exception error)
                {
                    logger.Log("Error caught while deleting cache");
                    exp = error;
                }
            }
            while (exp != null);
        }
    }
}
