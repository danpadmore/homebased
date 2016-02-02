namespace Homebase.Business.Repositories.Settings.Interfaces
{
    public interface IApplicationRepository
    {
        bool IsFirstTimeUserExperienceCompleted();
        void UpdateFirstTimeUserExperience(bool isCompleted);

        bool GetIsEnabled();
        void UpdateIsEnabled(bool isEnabled);
    }
}
