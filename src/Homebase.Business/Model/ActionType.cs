using System.Collections.Generic;

namespace Homebase.Business.Model
{
    public class ActionType
    {
        public int Identifer { get; set; }
        public string Name { get; set; }
        public DeviceCapability DeviceCapability { get; set; }

        public List<ActionArgument> ActionArguments { get; set; }

        public ActionType()
        {
            ActionArguments = new List<ActionArgument>();
        }
    }
}
