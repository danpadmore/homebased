using Windows.ApplicationModel.Background;

namespace Homebase.Business.Infrastructure.Interfaces
{
    public interface IBackgroundTaskRegistrar
    {
        BackgroundTaskRegistration Register(string taskEntryPoint, string name,
            IBackgroundTrigger trigger, IBackgroundCondition condition);

        void Unregister(string name);
    }
}
