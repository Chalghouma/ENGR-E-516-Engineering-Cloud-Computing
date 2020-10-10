using System;
using System.Collections.Generic;
using System.Text;

namespace MapReducer.Core
{
    public class WordCountMapper : IMapper<string, string, int>
    {
        public Dictionary<string, int> Map(string inputDataSet)
        {
            string[] words = inputDataSet.Split(" ");
            Dictionary<string, int> wordCount = new Dictionary<string, int>();
            foreach (var word in words)
            {
                if (!wordCount.ContainsKey(word))
                    wordCount[word] = 1;
                else
                    wordCount[word]++;
            }
            return wordCount;
        }
    }
}
