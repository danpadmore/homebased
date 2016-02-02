using System.Runtime.Serialization;

namespace Homebase.Business.Model
{
    [DataContract]
    public class ExceptionLog : Log
    {
        [DataMember]
        public string Error { get; set; }

        public override string Description { get { return Error.Substring(0, Error.Length > 20 ? 20 : Error.Length); } }
    }
}
