using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuStore.WebUI.Infrastructure.Abstract
{
    public interface ILogger
    {
        void LogInfo(string message);
        void LogError(string message);
        void LogWarning(string message);
        void LogException(Exception exception);
    }
}
