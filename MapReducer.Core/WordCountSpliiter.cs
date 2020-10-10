using MapReducer.Core.Logger;
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
            ILogger logger = new ConsoleLogger();
            string[] lines = await File.ReadAllLinesAsync(documentPath);
            var asyncMapTasks = lines.Select(line => new WordCountMapper("http://localhost:7071/api/WordCountMapper", logger).Map(line)).ToList();
            var mapTasksOutputs = await Task.WhenAll(asyncMapTasks);
            Dictionary<string, List<KeyValuePair<string, int>>> merged = new Dictionary<string, List<KeyValuePair<string, int>>>();
            foreach (var mapTaskOutput in mapTasksOutputs)
            {
                foreach (var pair in mapTaskOutput)
                {
                    if (!merged.ContainsKey(pair.Key))
                        merged[pair.Key] = new List<KeyValuePair<string, int>>();
                    merged[pair.Key].Add(pair);
                }
            }

            foreach (var pair in merged)
            {
                var red = new WordCountReducer();
                await red.Reduce(pair.Value);
            }
        }
    }
}
