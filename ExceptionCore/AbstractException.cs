using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionCore
{
    public abstract class AbstractException : Exception
    {
        protected new Exception InnerException { get; private set; }
        public string FriendlyMessage { get; protected set; }

        protected AbstractException(Exception exception)
        {
            InnerException = exception;
            FriendlyMessage = "Ha occurrido un error. Por favor contacta con el administrador del sistema";
        }

        protected AbstractException(string message)
        {
            FriendlyMessage = message;
            InnerException = null;
        }

        public abstract void LogError();
    }
}
