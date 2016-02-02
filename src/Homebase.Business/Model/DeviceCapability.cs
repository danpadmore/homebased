using System;
using System.Runtime.Serialization;

namespace Homebase.Business.Model
{
    [Flags]
    [DataContract]
    public enum DeviceCapability
    {
        [EnumMember]
        None,

        [EnumMember]
        Switchable,

        [EnumMember]
        WebService
    }
}
