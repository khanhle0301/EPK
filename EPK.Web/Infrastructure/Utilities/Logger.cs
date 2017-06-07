using System;
using log4net;
using log4net.Config;

namespace EPK.Web.Infrastructure.Utilities
{
    public class Logger
    {
        private const string LoggerName = "EPK_Logger";

        public static ILog FrameworkLogger { get; }

        static Logger()
        {
            XmlConfigurator.Configure();
            FrameworkLogger = LogManager.GetLogger(LoggerName);
        }

        public static void Error(object msg)
        {
            FrameworkLogger.Error(msg);
        }

        public static void Error(object msg, Exception e)
        {
            FrameworkLogger.Error(msg, e);
        }

        public static void Info(object msg)
        {
            FrameworkLogger.Info(msg);
        }

        public static void Info(object msg, Exception e)
        {
            FrameworkLogger.Info(msg, e);
        }
    }
}
