using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using Homebase.Business;
using Homebase.Business.Data.Interfaces;
using Homebase.Business.Services.Interfaces;
using Windows.ApplicationModel.Background;
using System.Diagnostics;

namespace Homebase.Background
{
    public sealed class GeofenceBackgroundTask : IBackgroundTask
    {
        private readonly IDataContextManager _dataContextManager;
        private readonly IActionExecutor _actionExecutor;
        private readonly ILogger _logger;

        public GeofenceBackgroundTask()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.ConfigureBusiness();

            _dataContextManager = SimpleIoc.Default.GetInstance<IDataContextManager>();
            _actionExecutor = SimpleIoc.Default.GetInstance<IActionExecutor>();
            _logger = SimpleIoc.Default.GetInstance<ILogger>();
        }

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            Debug.WriteLine($"{DateTime.Now} - run background task");

            try
            {
                await _dataContextManager.RefreshSettingsDataContext();
                await _dataContextManager.BeginTransaction();

                _actionExecutor.Execute();
            }
            catch(Exception ex)
            {
                _logger.Log(ex);
            }
            finally
            {
                await _dataContextManager.SaveChanges();

                deferral.Complete();
            }
        }
    }
}
