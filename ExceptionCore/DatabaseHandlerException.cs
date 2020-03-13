using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionCore
{
    public class DatabaseHandlerException : Critical
    {
        private const string message = "There is an error with database connection. Please try again later.";
        public DatabaseHandlerException(Exception exception) 
            : base(exception)
        {
            FriendlyMessage = message;
        }

        public override void LogError()
        {
            Logger.Instance.WriteError(FriendlyMessage, Severity.Fatal);
            Logger.Instance.WriteError(InnerException.Message, Severity.Fatal);
        }
    }
}
