using MapReducer.Core.Logger;
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
        public static async Task ProcessDocument(string documentPath)
        {
            string baseUrl = "http://localhost:7071/api";
            ILogger logger = new ConsoleLogger();
            string[] lines = await File.ReadAllLinesAsync(documentPath);
            var asyncMapTasks = lines.Select(line => new WordCountMapper($"{baseUrl}/WordCountMapper", logger).Map(line)).ToList();
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


            var asyncReduceTasks = keys.Select(key => new WordCountReducer("http://localhost:7071/api/WordCountReducer", logger).Reduce(key)).ToList();
            var reduceTasksOutputs = await Task.WhenAll(asyncReduceTasks);
            foreach (var reduceTaskoutput in reduceTasksOutputs)
            {
                string url = "http://localhost:7071/api/GetValueFunction";
                var resultValue = await RestClient.PostJson<StoredItem<int>>(new {key=reduceTaskoutput }, url);
                Console.WriteLine(JsonConvert.SerializeObject(resultValue));
            }
        }
    }
}
