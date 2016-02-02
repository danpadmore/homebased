using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Homebase.Business.Infrastructure.Interfaces;
using Homebase.Business.Model;
using Homebase.Business.Repositories.Fifthplay.Interfaces;
using System.Threading.Tasks;

namespace Homebase.Business.Repositories.Fifthplay
{
    internal class FifthplayRepository : IFifthplayRepository
    {
        private const string ApiBaseAddress = "https://s-api.fifthplay.com/Mobile/v4/";
        private static readonly XNamespace EnergyNamespace = "http://schemas.datacontract.org/2004/07/Fifthplay.API.Mobile.Contracts.v4.Data.Energy";
        private static readonly XNamespace ThermostatNamespace = "http://schemas.datacontract.org/2004/07/Fifthplay.API.Mobile.Contracts.v4.Data.Thermostat";
        private static readonly XNamespace DeviceNamespace = "http://schemas.datacontract.org/2004/07/Fifthplay.API.Mobile.Contracts.v4.Data.Device";
        private const string GetEnergyDevicesRequest = @"<s:Envelope xmlns:s=""http://www.w3.org/2003/05/soap-envelope"" xmlns:a=""http://www.w3.org/2005/08/addressing"" xmlns:u=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd""><s:Header><a:Action s:mustUnderstand=""1"">api.fifthplay.com/mobile/v4/IEnergyService/IEnergyService/GetEnergyDevices</a:Action><a:MessageID>urn:uuid:0f04d5a3-27cb-4e8c-b471-3fcf4d6289bc</a:MessageID><a:ReplyTo><a:Address>http://www.w3.org/2005/08/addressing/anonymous</a:Address></a:ReplyTo><a:To s:mustUnderstand=""1"">https://s-api.fifthplay.com/Mobile/v4/EnergyService.svc</a:To><o:Security s:mustUnderstand=""1"" xmlns:o=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd""><o:UsernameToken u:Id=""uuid-e95125a1-5c60-47e5-ab35-49e388ab3b1a-1""><o:Username>{username}</o:Username><o:Password Type=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">{password}</o:Password></o:UsernameToken></o:Security></s:Header><s:Body><GetEnergyDevices xmlns=""api.fifthplay.com/mobile/v4/IEnergyService""/></s:Body></s:Envelope>";
        private const string GetThermostatsRequest = @"<s:Envelope xmlns:s=""http://www.w3.org/2003/05/soap-envelope"" xmlns:a=""http://www.w3.org/2005/08/addressing"" xmlns:u=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd""><s:Header><a:Action s:mustUnderstand=""1"">api.fifthplay.com/mobile/v4/IThermostatService/IThermostatService/GetThermostatsDetails</a:Action><a:MessageID>urn:uuid:2dad2e6c-732f-4be5-bda2-9bc817af34da</a:MessageID><a:ReplyTo><a:Address>http://www.w3.org/2005/08/addressing/anonymous</a:Address></a:ReplyTo><a:To s:mustUnderstand=""1"">https://s-api.fifthplay.com/Mobile/v4/ThermostatService.svc</a:To><o:Security s:mustUnderstand=""1"" xmlns:o=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd""><o:UsernameToken u:Id=""uuid-2ba63a90-8b99-4201-9710-ca04b05b65c5-1""><o:Username>{username}</o:Username><o:Password Type=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">{password}</o:Password></o:UsernameToken></o:Security></s:Header><s:Body><GetThermostatsDetails xmlns=""api.fifthplay.com/mobile/v4/IThermostatService""/></s:Body></s:Envelope>";
        private const string GetDevicesRequest = @"<s:Envelope xmlns:s=""http://www.w3.org/2003/05/soap-envelope"" xmlns:a=""http://www.w3.org/2005/08/addressing"" xmlns:u=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd""><s:Header><a:Action s:mustUnderstand=""1"">api.fifthplay.com/mobile/v4/IDeviceService/IDeviceService/GetDevices</a:Action><a:MessageID>urn:uuid:8ca679bc-6c08-4600-a5ed-b335d362dd2f</a:MessageID><a:ReplyTo><a:Address>http://www.w3.org/2005/08/addressing/anonymous</a:Address></a:ReplyTo><a:To s:mustUnderstand=""1"">https://s-api.fifthplay.com/Mobile/v4/DeviceService.svc</a:To><o:Security s:mustUnderstand=""1"" xmlns:o=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd""><o:UsernameToken u:Id=""uuid-43a33f1f-6b18-401e-aa06-9db687625784-1""><o:Username>{username}</o:Username><o:Password Type=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">{password}</o:Password></o:UsernameToken></o:Security></s:Header><s:Body><GetDevices xmlns=""api.fifthplay.com/mobile/v4/IDeviceService""/></s:Body></s:Envelope>";
        private const string SwitchSmartPlugRequest = @"<s:Envelope xmlns:s=""http://www.w3.org/2003/05/soap-envelope"" xmlns:a=""http://www.w3.org/2005/08/addressing"" xmlns:u=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd""><s:Header><a:Action s:mustUnderstand=""1"">api.fifthplay.com/mobile/v4/IEnergyService/IEnergyService/SwitchSmartPlug</a:Action><a:MessageID>urn:uuid:59dd33ef-6d8b-41d7-8a99-b03d3cd789c1</a:MessageID><a:ReplyTo><a:Address>http://www.w3.org/2005/08/addressing/anonymous</a:Address></a:ReplyTo><a:To s:mustUnderstand=""1"">https://s-api.fifthplay.com/Mobile/v4/EnergyService.svc</a:To><o:Security s:mustUnderstand=""1"" xmlns:o=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd""><o:UsernameToken u:Id=""uuid-93d29271-aa33-48fb-a2ed-d1fb77750677-1""><o:Username>{username}</o:Username><o:Password Type=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">{password}</o:Password></o:UsernameToken></o:Security></s:Header><s:Body><SwitchSmartPlug xmlns=""api.fifthplay.com/mobile/v4/IEnergyService""><switchSmartPlugRequest xmlns:b=""http://schemas.datacontract.org/2004/07/Fifthplay.API.Mobile.Contracts.v1.Data.Energy"" xmlns:i=""http://www.w3.org/2001/XMLSchema-instance""><b:SmartPlugId>{smartplugid}</b:SmartPlugId><b:Value>{value}</b:Value></switchSmartPlugRequest></SwitchSmartPlug></s:Body></s:Envelope>";

        private readonly ISoapApiClient _soapApiClient;

        public FifthplayRepository(ISoapApiClient soapApiClient)
        {
            if (soapApiClient == null) throw new ArgumentNullException(nameof(soapApiClient));

            _soapApiClient = soapApiClient;
        }

        public async Task<IEnumerable<Device>> GetDevices(string username, string password)
        {
            var request = new ApiClientRequest
            {
                Username = username,
                Password = password,
                BaseAddress = ApiBaseAddress,
                RequestUri = "DeviceService.svc",
                Content = CreateRequestContent(GetDevicesRequest, username, password)
            };

            var response = await _soapApiClient.Request(request);
            var responseAsXml = XDocument.Parse(response);

            return responseAsXml
                .Descendants(DeviceNamespace + "DeviceDataContract")
                .Select(d => new Device
                {
                    Identifier = new Guid(d.Descendants(DeviceNamespace + "Uuid").Single().Value),
                    Name = d.Descendants(DeviceNamespace + "Name").Single().Value,
                    Capability = DeviceCapability.None,
                    Type = DeviceType.Fifthplay
                });
        }

        public async Task<IEnumerable<Device>> GetEnergyDevices(string username, string password)
        {
            var request = new ApiClientRequest
            {
                Username = username,
                Password = password,
                BaseAddress = ApiBaseAddress,
                RequestUri = "EnergyService.svc",
                Content = CreateRequestContent(GetEnergyDevicesRequest, username, password)
            };

            var response = await _soapApiClient.Request(request);
            var responseAsXml = XDocument.Parse(response);

            return responseAsXml
                .Descendants(EnergyNamespace + "EnergyDeviceDataContract")
                .Select(d => new Device
                {
                    Identifier = new Guid(d.Descendants(EnergyNamespace + "EnergyDeviceId").Single().Value),
                    Name = d.Descendants(EnergyNamespace + "Name").Single().Value,
                    Capability = bool.Parse(d.Descendants(EnergyNamespace + "IsSwitch").Single().Value)
                                ? DeviceCapability.Switchable
                                : DeviceCapability.None,
                    Type = DeviceType.Fifthplay
                });
        }

        public async Task<IEnumerable<Device>> GetThermostats(string username, string password)
        {
            var request = new ApiClientRequest
            {
                Username = username,
                Password = password,
                BaseAddress = ApiBaseAddress,
                RequestUri = "ThermostatService.svc",
                Content = CreateRequestContent(GetThermostatsRequest, username, password)
            };

            var response = await _soapApiClient.Request(request);
            var responseAsXml = XDocument.Parse(response);

            return responseAsXml
                .Descendants(EnergyNamespace + "ThermostatDetailsDataContract")
                .Select(d => new Device
                {
                    Identifier = new Guid(d.Descendants(ThermostatNamespace + "ThermostatId").Single().Value),
                    Name = d.Descendants(ThermostatNamespace + "Name").Single().Value,
                    Capability = DeviceCapability.None,
                    Type = DeviceType.Fifthplay
                });
        }

        public async Task SwitchSmartPlug(string username, string password, Guid identifier, bool isOn)
        {
            var request = new ApiClientRequest
            {
                Username = username,
                Password = password,
                BaseAddress = ApiBaseAddress,
                RequestUri = "EnergyService.svc",
                Content = CreateRequestContent(SwitchSmartPlugRequest, username, password)
                    .Replace("{smartplugid}", identifier.ToString())
                    .Replace("{value}", isOn.ToString().ToLowerInvariant())
            };

            var response = await _soapApiClient.Request(request);
            if (response.Contains("Fault"))
                throw new Exception("Failed to switch smart plug");
        }

        private string CreateRequestContent(string requestTemplate, string username, string password)
        {
            return requestTemplate
                .Replace("{username}", username)
                .Replace("{password}", password);
        }
    }
}
