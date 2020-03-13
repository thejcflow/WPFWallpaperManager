using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionCore
{
    public class Critical : AbstractException
    {
        public Critical(Exception exception)
            : base(exception)
        { }

        public override void LogError()
        {
            Logger.Instance.WriteError(FriendlyMessage, Severity.Error);
            Exception current = InnerException;
            do
            {
                string message = string.Format("{0}. {1}", current.Message, current.StackTrace);
                Logger.Instance.WriteError(message, Severity.Error);
                current = current.InnerException;
            } while (current != null);
        }
    }
}
