using MapReducer.Core;
using MapReducer.Core.InvertedIndex;
using System;
using System.IO;
using System.Linq;

namespace MapReduce.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string text = File.ReadAllText("Sample.txt");
            InvertedIndexSplitter.ProcessDocuments(new string[] { "Sample.txt" }, "http://localhost:7071/api").Wait();
            //WordCountSpliiter.ProcessDocument("Sample.txt", "http://localhost:7071/api").Wait();
        }
    }
}
