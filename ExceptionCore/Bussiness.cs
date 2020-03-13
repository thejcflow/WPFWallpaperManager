namespace ExceptionCore
{
    public class Bussiness : AbstractException
    {
        public Bussiness(string message, Severity severity = Severity.Warning)
          : base(message)
        { }

        public override void LogError()
        {
            Logger.Instance.WriteError(FriendlyMessage, Severity.Warning);
        }
    }
}
