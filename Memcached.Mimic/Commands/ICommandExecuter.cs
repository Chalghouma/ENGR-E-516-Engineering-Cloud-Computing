using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
        Task<ExecutionResult> ExecuteCommand(ICommand command);
        Task<ExecutionResult> ExecuteGetCommand(GetCommand command);
        Task<ExecutionResult> ExecuteSetCommand(SetCommand command);
        Task<ExecutionResult> ExecuteDeleteCommand(DeleteCommand command);
    }
}
