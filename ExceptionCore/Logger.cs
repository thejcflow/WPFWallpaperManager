using log4net;
using log4net.Config;

namespace ExceptionCore
{
    public class Logger
    {
        private static Logger instance;
        private readonly ILog config;

        public static Logger Instance
        {
            get
            {
                return instance ?? (instance = new Logger());
            }
        }
        private Logger()
        {
            config = LogManager.GetLogger("Logger");
            XmlConfigurator.Configure();
        }

        public void WriteError(string message, Severity severity)
        {
            switch (severity)
            {
                case Severity.Debug:
                    config.Debug(message);
                    break;
                case Severity.Information:
                    config.Info(message);
                    break;
                case Severity.Warning:
                    config.Warn(message);
                    break;
                case Severity.Error:
                    config.Error(message);
                    break;
                case Severity.Fatal:
                    config.Fatal(message);
                    break;
            }
        }        
    }
}
