using System;
using System.Runtime.Serialization;

namespace Homebase.Business.Model
{
    [DataContract]
    public class Action
    {
        [DataMember]
        public Guid Identifier { get; set; }

        [DataMember]
        public Guid DeviceIdentifier { get; set; }

        [DataMember]
        public int ActionTypeIdentifier { get; set; }

        [DataMember]
        public int ActionArgumentIdentifier { get; set; }

        [DataMember]
        public ActionTrigger ActionTrigger { get; set; }
    }
}
