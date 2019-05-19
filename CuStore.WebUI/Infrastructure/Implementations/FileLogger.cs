using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CuStore.WebUI.Infrastructure.Abstract;
using log4net;

namespace CuStore.WebUI.Infrastructure.Implementations
{
    public class FileLogger : ILogger
    {
        private readonly ILog _log;

        public FileLogger()
        {
            _log = LogManager.GetLogger("CuStore");
        }        

        public void LogInfo(string message)
        {
            _log.Info(message);
        }

        public void LogError(string message)
        {
            _log.Error(message);
        }

        public void LogWarning(string message)
        {
            _log.Warn(message);
        }

        public void LogException(Exception exception)
        {
            _log.Error(exception.Message, exception);
        }
    }
}