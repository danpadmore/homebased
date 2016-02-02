namespace Homebase.Business.Repositories.Ifttt.Interfaces
{
    public interface IIftttRepository
    {
        void TriggerEvent(string key, string @event, string location, string value2 = null, string value3 = null);
    }
}
