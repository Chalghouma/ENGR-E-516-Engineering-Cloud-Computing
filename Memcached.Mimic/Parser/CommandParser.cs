using Memcached.Mimic.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Memcached.Mimic.Parser
{
    public class CommandParser
    {
        public static ICommand ParseFromSentData(string sentData)
        {
            string trimmed = sentData.Trim();
            if (trimmed.StartsWith("get"))
                return GetCommandFromInput(trimmed.Remove(0, "get".Length));
            else if (trimmed.StartsWith("set"))
                return SetCommandFromSentData(trimmed.Remove(0, "set".Length));

            return null;
        }

        public static ICommand ParseFromUserInput(string userInput, bool requireUserInput, string additionalUserInput = "")
        {
            string trimmed = userInput.Trim();
            if (trimmed.StartsWith("get"))
                return GetCommandFromInput(trimmed.Remove(0, "get".Length));
            else if (trimmed.StartsWith("set"))
                return SetCommandFromInput(trimmed.Remove(0, "set".Length),
                    requireUserInput ? Console.ReadLine() : additionalUserInput);

            return null;
        }


        private static ICommand GetCommandFromInput(string headLess)
        {
            return new GetCommand(headLess.Trim());
        }

        private static ICommand SetCommandFromInput(string headLess, string additionalLine)
        {
            string trimmed = headLess.Trim();
            string key = trimmed.Split(' ')[0];
            trimmed = trimmed.Remove(0, key.Length);
            trimmed = trimmed.Trim();
            int valueSizesBytes = 0;
            int.TryParse(trimmed.Split(' ')[0], out valueSizesBytes);
            return new SetCommand(key, valueSizesBytes, additionalLine);
        }
        private static ICommand SetCommandFromSentData(string headLess)
        {
            string trimmed = headLess.Trim();
            string key = trimmed.Split(' ')[0];
            trimmed = trimmed.Remove(0, key.Length);
            trimmed = trimmed.Trim();
            int valueSizesBytes = 0;
            int.TryParse(trimmed.Split(' ')[0], out valueSizesBytes);
            trimmed = trimmed.Remove(0, trimmed.Split(' ')[0].Length);
            trimmed = trimmed.Trim();
            return new SetCommand(key, valueSizesBytes, trimmed);
        }
    }
}
