using System;
using System.Runtime.Serialization;

namespace Homebase.Business.Model
{
    [DataContract]
    public class Device
    {
        [DataMember]
        public Guid Identifier { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DeviceCapability Capability { get; set; }

        [DataMember]
        public DeviceType Type { get; set; }
    }
}
