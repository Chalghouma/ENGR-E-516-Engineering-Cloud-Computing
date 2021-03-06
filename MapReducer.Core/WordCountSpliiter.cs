﻿using MapReducer.Core.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapReducer.Core
{
    public class WordCountSpliiter
    {
        public static async Task ProcessDocument(string documentPath, string azureFunctionsBaseUrl, ILogger logger)
        {
            logger.Log($"Processing Document at {documentPath}");
            await DeleteCache(logger, azureFunctionsBaseUrl);
            string[] lines = await File.ReadAllLinesAsync(documentPath);
            var asyncMapTasks = lines.Select(line => Task.Run(() => new WordCountMapper($"{azureFunctionsBaseUrl}/WordCountMapper", logger).Map(line))
            ).ToList();
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


            var asyncReduceTasks = keys.Select(key => Task.Run(() => new WordCountReducer($"{azureFunctionsBaseUrl}/WordCountReducer", logger).Reduce(key))
            ).ToList();
            var reduceTasksOutputs = await Task.WhenAll(asyncReduceTasks);
            logger.Log("All Reducers have ended");
            logger.Log("Starting to get end-results from KeyStore");
            foreach (var reduceTaskoutput in reduceTasksOutputs)
            {
                string url = $"{azureFunctionsBaseUrl}/GetValueFunction";
                var resultValue = await RestClient.PostJson<StoredItem<List<int>>>(new { key = reduceTaskoutput }, url);
                logger.Log($"{resultValue.key.Replace("_REDUCER_OUTPUT", "")} has Value = {JsonConvert.SerializeObject(resultValue.value[0])}");
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
