using System;

namespace ToDo.Common.Exceptions
{
    public class InvalidOperationExceptions : TazeezException
    {
        public InvalidOperationExceptions() : base("Service Validation Exception")
        {
        }

        public InvalidOperationExceptions(string message) : base(message)
        {
        }

        public InvalidOperationExceptions(int code, string message) : base(code, message)
        {
        }

        public InvalidOperationExceptions(string message, Exception ex) : base(message, ex)
        {
        }

        public InvalidOperationExceptions(int code, string message, Exception ex) : base(code, message, ex)
        {
        }
    }
}
