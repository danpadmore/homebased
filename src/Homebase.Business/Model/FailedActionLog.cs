using System;
using System.Runtime.Serialization;

namespace Homebase.Business.Model
{
    [DataContract]
    public class FailedActionLog : ActionLog
    {
        [DataMember]
        public string Error { get; set; }

        public override string Description
        {
            get
            {
                return $"Failed to control {DeviceName}{Environment.NewLine}This is what happened: {Error.Substring(0, Error.Length > 20 ? 20 : Error.Length)}";
            }
        }
    }
}
