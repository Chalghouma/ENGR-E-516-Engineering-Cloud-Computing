using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Memcached.Mimic.Commands
{
    public struct ExecutionResult<T>
    {
        public bool IsSuccess { get; set; }
        public T Result { get; set; }
        public float ExecutionTimeInMS { get; set; }
    }
    public interface ICommandExecuter<T>
    {
        Task<ExecutionResult<T>> ExecuteCommand(ICommand command);
        Task<ExecutionResult<T>> ExecuteGetCommand(GetCommand command);
        Task<ExecutionResult<T>> ExecuteSetCommand(SetCommand command);
        Task<ExecutionResult<T>> ExecuteDeleteCommand(DeleteCommand command);
    }
}
