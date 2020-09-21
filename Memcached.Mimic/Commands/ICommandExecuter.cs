using System;
using System.Collections.Generic;
using System.Text;

namespace Memcached.Mimic.Commands
{
    public struct ExecutionResult
    {
        public bool IsSuccess { get; set; }
        public string[] Results { get; set; }
        public float ExecutionTimeInMS { get; set; }
    }
    public interface ICommandExecuter
    {
        ExecutionResult ExecuteCommand(ICommand command);
        ExecutionResult ExecuteGetCommand(GetCommand command);
        ExecutionResult ExecuteSetCommand(SetCommand command);
        ExecutionResult ExecuteDeleteCommand(DeleteCommand command);
    }
}
