using MapReducer.Core;
using System;
using System.Linq;

namespace MapReduce.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            WordCountSpliiter.ProcessDocument("Sample.txt","http://localhost:7071/api").Wait();
            Console.WriteLine("Hello World!");
        }
    }
}
