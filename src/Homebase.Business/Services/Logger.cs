using System;
using Homebase.Business.Model;
using Homebase.Business.Repositories.Settings.Interfaces;
using Homebase.Business.Services.Interfaces;

namespace Homebase.Business.Services
{
    internal class Logger : ILogger
    {
        private readonly ILogRepository _logRepository;
        private readonly IDeviceRepository _deviceRepository;

        public Logger(ILogRepository logRepository, IDeviceRepository deviceRepository)
        {
            if (logRepository == null) throw new ArgumentNullException(nameof(logRepository));
            if (deviceRepository == null) throw new ArgumentNullException(nameof(_deviceRepository));

            _logRepository = logRepository;
            _deviceRepository = deviceRepository;
        }

        public void Log(ActionToExecute actionToExecute, DateTimeOffset timestamp)
        {
            var log = new ActionLog
            {
                Identifier = Guid.NewGuid(),
                DeviceName = actionToExecute.DeviceName,
                ActionTypeName = actionToExecute.ActionTypeName,
                Type = actionToExecute.ActionTriggerValue,
                ActionArgumentValue = actionToExecute.ActionArgumentValue,
                Timestamp = timestamp
            };

            _logRepository.Add(log);
        }

        public void Log(Exception ex, ActionToExecute failedAction, DateTimeOffset timestamp)
        {
            var log = new FailedActionLog
            {
                Identifier = Guid.NewGuid(),
                DeviceName = failedAction.DeviceName,
                ActionTypeName = failedAction.ActionTypeName,
                Type = failedAction.ActionTriggerValue,
                ActionArgumentValue = failedAction.ActionArgumentValue,
                Error = ex.Message,
                Timestamp = timestamp
            };

            _logRepository.Add(log);
        }

        public void Log(Exception ex)
        {
            var log = new ExceptionLog
            {
                Identifier = Guid.NewGuid(),
                Error = ex.Message,
                Timestamp = DateTime.UtcNow,
                Type = "error"
            };

            _logRepository.Add(log);
        }
    }
}