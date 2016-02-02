using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using Homebase.Business.Data;
using Homebase.Business.Data.Interfaces;
using Homebase.Business.Infrastructure;
using Homebase.Business.Model;

namespace Homebase.Tests.Integration.Data.DataContextManagerSpecifications
{
    [TestClass]
    public class WhenSaveChanges : TestBase
    {
        protected IDataContextManager DataContextManager { get; private set; }

        protected override void Arrange()
        {
            DataContextManager = new DataContextManager();
            DataContextManager.RefreshSettingsDataContext().Wait();

            var switchableDevice = new Device { Identifier = Guid.NewGuid(), Name = "switchable device", Capability = DeviceCapability.Switchable, Type = DeviceType.Fifthplay };
            var unsupportedDevice = new Device { Identifier = Guid.NewGuid(), Name = "unsupported device", Capability = DeviceCapability.None, Type = DeviceType.Ifttt };

            DataContextManager.SettingsDataContext.IsApplicationEnabled = true;
            DataContextManager.SettingsDataContext.Devices = new List<Device> { switchableDevice, unsupportedDevice };

            DataContextManager.SettingsDataContext.Actions = new List<Business.Model.Action>
            {
                new Business.Model.Action
                {
                    Identifier = Guid.NewGuid(),
                    DeviceIdentifier = switchableDevice.Identifier,
                    ActionTypeIdentifier = DataContextManager.SettingsDataContext.ActionTypes.First().Identifer,
                    ActionArgumentIdentifier = DataContextManager.SettingsDataContext.ActionTypes.First().ActionArguments.First().Identifer,
                    ActionTrigger = ActionTrigger.Home
                },
                new Business.Model.Action
                {
                    Identifier = Guid.NewGuid(),
                    DeviceIdentifier = unsupportedDevice.Identifier,
                    ActionTypeIdentifier = DataContextManager.SettingsDataContext.ActionTypes.First().Identifer,
                    ActionArgumentIdentifier = DataContextManager.SettingsDataContext.ActionTypes.First().ActionArguments.ElementAt(1).Identifer,
                    ActionTrigger = ActionTrigger.Away
                }
            };

            DataContextManager.SettingsDataContext.Logs.Add(new ActionLog { Identifier = Guid.NewGuid(), ActionArgumentValue = "on", Type = "Home", ActionTypeName = "switch", DeviceName = "dummy device", Timestamp = DateTimeOffset.UtcNow });
            DataContextManager.SettingsDataContext.Logs.Add(new ActionLog { Identifier = new Guid("6fd4aa18-e36c-493c-815d-d4e1e8ed675f"), ActionArgumentValue = "on", Type = "Home", ActionTypeName = "switch", DeviceName = "Desk lamp", Timestamp = DateTimeOffset.UtcNow });
            DataContextManager.SettingsDataContext.Logs.Add(new FailedActionLog { Identifier = Guid.NewGuid(), ActionArgumentValue = "on", Type = "Home", ActionTypeName = "switch", DeviceName = "dummy device", Timestamp = DateTimeOffset.UtcNow, Error = "dummy-error" });
            DataContextManager.SettingsDataContext.Logs.Add(new FailedActionLog { Identifier = Guid.NewGuid(), ActionArgumentValue = "on", Type = "Home", ActionTypeName = "switch", DeviceName = "dummy device", Timestamp = DateTimeOffset.UtcNow, Error = "dummy-error" });
            DataContextManager.SettingsDataContext.Logs.Add(new ExceptionLog { Identifier = Guid.NewGuid(), Error = new Exception().ToString(), Timestamp = DateTimeOffset.UtcNow });
            DataContextManager.SettingsDataContext.Logs.Add(new ExceptionLog { Identifier = Guid.NewGuid(), Error = "dummy-error", Timestamp = DateTimeOffset.UtcNow });
        }

        protected override void Act()
        {
            DataContextManager.SaveChanges().Wait();
        }

        [TestMethod]
        public void ThenSettingsDataContextShouldBeSerialized()
        {
            var result = StorageHelper.Load<SettingsDataContext>("settingsdatacontext.db").Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(DataContextManager.SettingsDataContext.IsApplicationEnabled, result.IsApplicationEnabled);
            Assert.AreEqual(DataContextManager.SettingsDataContext.Devices.Count, result.Devices.Count);
            Assert.AreEqual(DataContextManager.SettingsDataContext.Logs.Count, result.Logs.Count);

            foreach (var device in DataContextManager.SettingsDataContext.Devices)
            {
                var resultDevice = result.Devices.SingleOrDefault(d => d.Identifier == device.Identifier);
                AssertDevice(device, resultDevice);
            }

            foreach (var action in DataContextManager.SettingsDataContext.Actions)
            {
                var resultAction = result.Actions.SingleOrDefault(d => d.Identifier == action.Identifier);

                Assert.IsNotNull(resultAction);
                Assert.AreEqual(action.Identifier, resultAction.Identifier);
                Assert.AreEqual(action.ActionArgumentIdentifier, resultAction.ActionArgumentIdentifier);
                Assert.AreEqual(action.ActionTrigger, resultAction.ActionTrigger);
                Assert.AreEqual(action.ActionTypeIdentifier, resultAction.ActionTypeIdentifier);
                Assert.AreEqual(action.DeviceIdentifier, resultAction.DeviceIdentifier);
            }

            foreach (var log in DataContextManager.SettingsDataContext.Logs)
            {
                var resultLog = result.Logs.SingleOrDefault(d => d.Identifier == log.Identifier);

                Assert.IsNotNull(resultLog);
                Assert.IsInstanceOfType(resultLog, log.GetType());
                Assert.AreEqual(log.Description, log.Description);
                Assert.AreEqual(log.Timestamp, log.Timestamp);
            }
        }

        private void AssertDevice(Device original, Device compare)
        {
            Assert.IsNotNull(compare);
            Assert.AreEqual(original.Identifier, compare.Identifier);
            Assert.AreEqual(original.Name, compare.Name);
            Assert.AreEqual(original.Capability, compare.Capability);
        }
    }
}
