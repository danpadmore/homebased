using System;

namespace Homebase.Business.Model
{
    public class ActionDescription
    {
        public Guid ActionIdentifier { get; set; }
        public Guid DeviceIdentifier { get; set; }
        public int ActionTypeIdentifier { get; set; }
        public int ActionArgumentIdentifier { get; set; }
        public ActionTrigger ActionTrigger { get; set; }

        public string DeviceName { get; set; }
        public string Result { get; set; }
        public string ActionTriggerName
        {
            get { return ActionTrigger.ToString(); }
        }
    }
}
