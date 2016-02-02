using System;
using System.Runtime.Serialization;

namespace Homebase.Business.Model
{
    [DataContract]
    public class ActionLog : Log
    {
        [DataMember]
        public string DeviceName { get; set; }

        [DataMember]
        public string ActionTypeName { get; set; }

        [DataMember]
        public string ActionArgumentValue { get; set; }

        public override string Description
        {
            get
            {
                return $"{ActionTypeName} {ActionArgumentValue} {DeviceName}";
            }
        }
    }
}
