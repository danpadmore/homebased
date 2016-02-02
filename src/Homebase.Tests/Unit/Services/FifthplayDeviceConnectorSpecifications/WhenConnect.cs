using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using Homebase.Business.Model;
using Homebase.Business.Repositories.Interfaces.Settings;
using Homebase.Business.Repositories.Settings.Interfaces;
using Homebase.Business.Services;
using Homebase.Business.Services.Interfaces;
using Homebase.Tests.Mocks;

namespace Homebase.Tests.Unit.Services.FifthplayDeviceConnectorSpecifications
{
    public abstract class WhenConnect : TestBase
    {
        public ICredentialsRepository CredentialsRepository { get; private set; }
        public IDeviceRepository DeviceRepository { get; private set; }
        public MockFifthplayRepository FifthplayRepository { get; private set; }
        public IDeviceConnector Service { get; private set; }
        public ConnectDeviceRequest Request { get; set; }

        protected override void Arrange()
        {
            CredentialsRepository = new MockCredentialRepository();
            DeviceRepository = new MockDeviceRepository();
            FifthplayRepository = new MockFifthplayRepository();

            Service = new FifthplayDeviceConnector(
                FifthplayRepository, CredentialsRepository, DeviceRepository);

            Request = new ConnectDeviceRequest
            {
                Username = "dummy-user",
                Password = "dummy-password"
            };
        }

        protected override void Act()
        {
            Service.Connect(Request);
        }
    }

    [TestClass]
    public class WhenConnectGivenRequestIsEmpty : WhenConnect
    {
        protected override void Arrange()
        {
            base.Arrange();
            IsExceptionExpected = true;
            Request = null;
        }

        [TestMethod]
        public void ThenAnArgumentNullExceptionShouldBeThrown()
        {
            var ex = Exception as ArgumentNullException;
            Assert.IsNotNull(ex);
            Assert.AreEqual("request", ex.ParamName);
        }
    }

    [TestClass]
    public class WhenConnectGivenUsernameIsEmpty : WhenConnect
    {
        protected override void Arrange()
        {
            base.Arrange();
            IsExceptionExpected = true;
            Request.Username = null;
        }

        [TestMethod]
        public void ThenAnArgumentExceptionShouldBeThrown()
        {
            var ex = Exception as ArgumentException;
            Assert.IsNotNull(ex);
            Assert.AreEqual("request", ex.ParamName);
        }
    }

    [TestClass]
    public class WhenConnectGivenPasswordIsEmpty : WhenConnect
    {
        protected override void Arrange()
        {
            base.Arrange();
            IsExceptionExpected = true;
            Request.Password = null;
        }

        [TestMethod]
        public void ThenAnArgumentExceptionShouldBeThrown()
        {
            var ex = Exception as ArgumentException;
            Assert.IsNotNull(ex);
            Assert.AreEqual("request", ex.ParamName);
        }
    }

    [TestClass]
    public class WhenConnectGivenRequestIsValid : WhenConnect
    {
        [TestMethod]
        public void ThenEnergyDevicesShouldBeRetrieved()
        {
            Assert.AreEqual(1, FifthplayRepository.GetEnergyDevicesCallCount);
        }

        [TestMethod]
        public void ThenThermostatsShouldBeRetrieved()
        {
            Assert.AreEqual(1, FifthplayRepository.GetThermostatsCallCount);
        }

        [TestMethod]
        public void ThenDevicesShouldBeRetrieved()
        {
            Assert.AreEqual(1, FifthplayRepository.GetDevicesCallCount);
        }
    }
}
