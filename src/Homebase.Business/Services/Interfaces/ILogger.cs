using System;
using Homebase.Business.Model;

namespace Homebase.Business.Services.Interfaces
{
    public interface ILogger
    {
        void Log(Exception ex);
        void Log(ActionToExecute actionToExecute, DateTimeOffset timestamp);
        void Log(Exception ex, ActionToExecute failedAction, DateTimeOffset timestamp);
    }
}
