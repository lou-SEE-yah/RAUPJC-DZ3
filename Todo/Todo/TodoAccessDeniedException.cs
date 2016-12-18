using System;
using System.Runtime.Serialization;

namespace Todo
{
    [Serializable]
    internal class TodoAccessDeniedException : Exception
    {
        public TodoAccessDeniedException()
        {
        }

        public TodoAccessDeniedException(string message) : base(message)
        {
        }

        public TodoAccessDeniedException(string message, Exception innerException) : base(message, innerException)
        {
        }
        
        public TodoAccessDeniedException(Guid userId, Guid todoId, Exception innerException = null) : base($"User with ID: {userId} tried to access {todoId} without permission.", innerException)
        {
        }

        protected TodoAccessDeniedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}