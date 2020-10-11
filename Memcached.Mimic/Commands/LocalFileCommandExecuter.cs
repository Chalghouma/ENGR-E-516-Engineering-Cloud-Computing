using Memcached.Mimic.FileHandler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Memcached.Mimic.Commands
{
    public class LocalFileCommandExecuter : ICommandExecuter
    {
        IFileHandler _fileHandler;
        public LocalFileCommandExecuter(IFileHandler fileHandler)
        {
            _fileHandler = fileHandler;
        }

        public async Task<ExecutionResult> ExecuteCommand(ICommand command)
        {
            try
            {
                if (command is GetCommand)
                    return await ExecuteGetCommand(command as GetCommand);
                else if (command is SetCommand)
                    return await ExecuteSetCommand(command as SetCommand);
                else if (command is DeleteCommand)
                    return await ExecuteDeleteCommand(command as DeleteCommand);
                else
                    return new ExecutionResult
                    {
                        IsSuccess = false,
                        Results = new string[] { "Incoming Command didn't map to any existing command", },
                        ExecutionTimeInMS = -1000

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
                    },
                    ExecutionTimeInMS = -1000
                };
            }
        }

        public async Task<ExecutionResult> ExecuteDeleteCommand(DeleteCommand command)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            bool result = _fileHandler.DeleteKey(command.Key);
            stopWatch.Stop();
            return new ExecutionResult
            {
                IsSuccess = true,
                Results = new string[]
                {
                    result ? "DELETED" : "NOT FOUND"
                },
                ExecutionTimeInMS=stopWatch.ElapsedMilliseconds
            };
        }

        public async Task<ExecutionResult> ExecuteGetCommand(GetCommand command)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            string keyValue = "";
            bool result = _fileHandler.GetKeyValue(command.Key, out keyValue);
            int length = result ? keyValue.Length : 0;
            stopWatch.Stop();
            return new ExecutionResult
            {
                IsSuccess = true,
                Results = new string[] {
                    $"VALUE {command.Key} {length}",
                    keyValue,
                    "END"
                },
                ExecutionTimeInMS = stopWatch.ElapsedMilliseconds
            };
        }


        public async Task<ExecutionResult> ExecuteSetCommand(SetCommand command)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            bool result = _fileHandler.SetKey(command.Key, command.Value);
            stopWatch.Stop();
            return new ExecutionResult
            {
                IsSuccess = true,
                Results = new string[] { result ? "STORED" : "NOT STORED" },
                ExecutionTimeInMS = stopWatch.ElapsedMilliseconds
            };
        }
    }
}
