using System;
using System.Runtime.Serialization;

namespace Homebase.Business.Model
{
    [KnownType(typeof(ActionLog))]
    [KnownType(typeof(FailedActionLog))]
    [KnownType(typeof(ExceptionLog))]
    [DataContract]
    public abstract class Log
    {
        [DataMember]
        public Guid Identifier { get; set; }

        [DataMember]
        public DateTimeOffset Timestamp { get; set; }

        [DataMember]
        public string Type { get; set; }


        public abstract string Description { get; }
    }
}
