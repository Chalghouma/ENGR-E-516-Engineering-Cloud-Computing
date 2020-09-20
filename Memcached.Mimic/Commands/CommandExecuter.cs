using Memcached.Mimic.FileHandler;
using System;
using System.Collections.Generic;
using System.Text;

namespace Memcached.Mimic.Commands
{
    public class CommandExecuter : ICommandExecuter
    {
        IFileHandler _fileHandler;
        public CommandExecuter()
        {
            _fileHandler = new TextFileHandler();
        }

        public ExecutionResult ExecuteCommand(ICommand command)
        {
            try
            {
                if (command is GetCommand)
                    return ExecuteGetCommand(command as GetCommand);
                else if (command is SetCommand)
                    return ExecuteSetCommand(command as SetCommand);
                else if (command is DeleteCommand)
                    return ExecuteDeleteCommand(command as DeleteCommand);
                else
                    return new ExecutionResult
                    {
                        IsSuccess = false,
                        Results = new string[] { "Incoming Command didn't map to any existing command" }
                    };
            }
            catch (Exception exp)
            {
                return new ExecutionResult
                {
                    IsSuccess = false,
                    Results = new string[]
                    {
                        "An error Occured while executing your command",
                        $"Exception Message : {exp.Message}",
                        $"Exception StackTrace:  {exp.StackTrace}"
                    }
                };
            }
        }

        public ExecutionResult ExecuteDeleteCommand(DeleteCommand command)
        {
            bool result = _fileHandler.DeleteKey(command.Key);
            return new ExecutionResult
            {
                IsSuccess = true,
                Results = new string[]
                {
                    result ? "DELETED" : "NOT FOUND"
                }
            };
        }

        public ExecutionResult ExecuteGetCommand(GetCommand command)
        {
            string keyValue = "";
            bool result = _fileHandler.GetKeyValue(command.Key, out keyValue);
            int length = result ? keyValue.Length : 0;
            return new ExecutionResult
            {
                IsSuccess = true,
                Results = new string[] {
                    $"VALUE {command.Key} {length}\n",
                    keyValue+"\n",
                    "END"
                }
            };
        }


        public ExecutionResult ExecuteSetCommand(SetCommand command)
        {
            bool result = _fileHandler.SetKey(command.Key, command.Value);
            return new ExecutionResult
            {
                IsSuccess = true,
                Results = new string[] { result ? "STORED" : "NOT STORED" }
            };
        }
    }
}
