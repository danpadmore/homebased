using System;

namespace Homebase.Business.Model
{
    public class ActionToExecute
    {
        public Guid DeviceIdentifier { get; internal set; }
        public string DeviceName { get; set; }
        public string ActionTypeName { get; set; }
        public string ActionArgumentValue { get; set; }
        public string ActionTriggerValue { get; set; }
    }
}
