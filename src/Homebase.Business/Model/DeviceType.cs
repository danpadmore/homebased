using System.Runtime.Serialization;

namespace Homebase.Business.Model
{
    [DataContract]
    public enum DeviceType
    {
        [EnumMember]
        Fifthplay,

        [EnumMember]
        Ifttt
    }
}
