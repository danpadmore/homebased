namespace Homebase.Business.Services.Interfaces
{
    public interface IFirstTimeUserExperienceTracker
    {
        /// <returns>True when first time user experience is completed</returns>
        bool Track();
    }
}
