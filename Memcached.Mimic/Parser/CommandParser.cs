using Memcached.Mimic.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Memcached.Mimic.Parser
{
    public class CommandParser
    {
        public static ICommand Parse(string userInput)
        {
            string trimmed = userInput.Trim();
            if (trimmed.StartsWith("get"))
                return GetCommandFromInput(trimmed.Remove(0, "get".Length));
            else if (trimmed.StartsWith("set"))
                return null;

            return null;
        }

        private static ICommand GetCommandFromInput(string headLess)
        {
            return new GetCommand(headLess.Trim());
        }
    }
}
