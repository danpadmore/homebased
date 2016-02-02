using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Homebase.Business.Model;

namespace Homebase.Business.Data
{
    [DataContract]
    public class SettingsDataContext
    {
        private static readonly Lazy<List<ActionType>> _actionTypes;

        [DataMember]
        public bool IsFirstTimeUserExperienceCompleted { get; internal set; }

        [DataMember]
        public bool IsApplicationEnabled { get; set; }

        [DataMember]
        public List<Device> Devices { get; set; }

        [DataMember]
        public List<Model.Action> Actions { get; set; }

        [DataMember]
        public List<Log> Logs { get; set; }

        public IReadOnlyCollection<ActionType> ActionTypes { get { return _actionTypes.Value; } }

        static SettingsDataContext()
        {
            _actionTypes = new Lazy<List<ActionType>>(() => LoadActionTypes());
        }

        public SettingsDataContext()
        {
            Devices = new List<Device>();
            Actions = new List<Model.Action>();
            Logs = new List<Log>();
        }

        private static List<ActionType> LoadActionTypes()
        {
            var on = new ActionArgument { Identifer = 1, Value = "on" };
            var off = new ActionArgument { Identifer = 2, Value = "off" };
            var location = new ActionArgument { Identifer = 3, Value = "location" };

            return new List<ActionType>
            {
                new ActionType
                {
                    Identifer = 1, Name = "switch",
                    DeviceCapability = DeviceCapability.Switchable,
                    ActionArguments = new List<ActionArgument> { on, off }
                },
                new ActionType
                {
                    Identifer = 2, Name = "IFTTT Maker Event",
                    DeviceCapability = DeviceCapability.WebService,
                    ActionArguments = new List<ActionArgument> { location }
                }
            };
        }
    }
}
