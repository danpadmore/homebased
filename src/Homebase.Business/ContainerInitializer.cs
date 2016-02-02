using GalaSoft.MvvmLight.Ioc;
using Homebase.Business.Data;
using Homebase.Business.Data.Interfaces;
using Homebase.Business.Infrastructure;
using Homebase.Business.Infrastructure.Interfaces;
using Homebase.Business.Repositories.Fifthplay;
using Homebase.Business.Repositories.Fifthplay.Interfaces;
using Homebase.Business.Repositories.Ifttt;
using Homebase.Business.Repositories.Ifttt.Interfaces;
using Homebase.Business.Repositories.Interfaces.Settings;
using Homebase.Business.Repositories.Settings;
using Homebase.Business.Repositories.Settings.Interfaces;
using Homebase.Business.Repositories.System;
using Homebase.Business.Repositories.System.Interfaces;
using Homebase.Business.Services;
using Homebase.Business.Services.Interfaces;

namespace Homebase.Business
{
    public static class ContainerInitializer
    {
        public static SimpleIoc ConfigureBusiness(this SimpleIoc simpleIoc)
        {
            return simpleIoc
                .RegisterInfrastructure()
                .RegisterData()
                .RegisterRepositories()
                .RegisterServices();
        }

        private static SimpleIoc RegisterServices(this SimpleIoc simpleIoc)
        {
            simpleIoc.Register<IFirstTimeUserExperienceTracker, FirstTimeUserExperienceTracker>();
            simpleIoc.Register<IDeviceConnector, FifthplayDeviceConnector>();
            simpleIoc.Register<IFunctionalityToggler, FunctionalityToggler>();
            simpleIoc.Register<IHomeLocationUpdater, HomeLocationUpdater>();
            simpleIoc.Register<IActionExecutor, ActionExecutor>();
            simpleIoc.Register<ILogger, Logger>();
            simpleIoc.Register<IIftttConnector, IftttConnector>();
            simpleIoc.Register<IActionUpdater, ActionUpdater>();

            return simpleIoc;
        }

        private static SimpleIoc RegisterRepositories(this SimpleIoc simpleIoc)
        {
            simpleIoc.Register<IApplicationRepository, ApplicationRepository>();
            simpleIoc.Register<IDeviceRepository, DeviceRepository>();
            simpleIoc.Register<IActionRepository, ActionRepository>();
            simpleIoc.Register<IActionTypeRepository, ActionTypeRepository>();
            simpleIoc.Register<ILogRepository, LogRepository>();

            simpleIoc.Register<IFifthplayRepository, FifthplayRepository>();
            simpleIoc.Register<IIftttRepository, IftttRepository>();

            simpleIoc.Register<ICredentialsRepository, CredentialsRepository>();

            simpleIoc.Register<IPermissionRepository, PermissionRepository>();
            simpleIoc.Register<ILocationRepository, LocationRepository>();

            return simpleIoc;
        }

        private static SimpleIoc RegisterInfrastructure(this SimpleIoc simpleIoc)
        {
            simpleIoc.Register<ISoapApiClient, SoapApiClient>();
            simpleIoc.Register<IBackgroundTaskRegistrar, BackgroundTaskRegistrar>();

            return simpleIoc;
        }

        private static SimpleIoc RegisterData(this SimpleIoc simpleIoc)
        {
            simpleIoc.Register<IDataContextManager, DataContextManager>();

            return simpleIoc;
        }
    }
}
