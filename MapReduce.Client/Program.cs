using MapReducer.Core;
using MapReducer.Core.InvertedIndex;
using MapReducer.Core.Logger;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace MapReduce.Client
{
    public enum ReduceFunction
    {
        WORD_COUNT,
        INVERTED_INDEX
    }
    public class Config
    {
        public ReduceFunction ReduceFunction;
        public string AzureFunctionsEndpoint;
        public string WordCountFilePath;
        public string[] InvertedIndexFilePaths;
    }
    class Program
    {
        static void Main(string[] args)
        {
            Config config = null;
            do
            {
                try
                {
                    Console.WriteLine("Please, enter a valid path for the config file");
                    string configFilePath = Console.ReadLine();
                    config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configFilePath));
                }
                catch (Exception exp) { }
            }
            while (config == null);

            string guid = Guid.NewGuid().ToString();
            string logFileName = $"LogFile_{guid}.txt";
            ILogger logger = new ConsoleFileLogger(logFileName);
            logger.Log($"A log file has been created at {logFileName}");

            if (config.ReduceFunction == ReduceFunction.WORD_COUNT)
            {
                WordCountSpliiter.ProcessDocument(config.WordCountFilePath, config.AzureFunctionsEndpoint,logger).Wait();
            }
            else
                InvertedIndexSplitter.ProcessDocuments(config.InvertedIndexFilePaths, config.AzureFunctionsEndpoint,logger).Wait();
        }
    }
}
