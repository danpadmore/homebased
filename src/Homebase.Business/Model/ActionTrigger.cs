using System.Runtime.Serialization;

namespace Homebase.Business.Model
{
    [DataContract]
    public enum ActionTrigger : int
    {
        [EnumMember]
        Home = 1,

        [EnumMember]
        Away = 2
    }
}
