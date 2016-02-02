using Homebase.Business.Data.Interfaces;
using Homebase.Business.Repositories.Settings.Interfaces;

namespace Homebase.Business.Repositories.Settings
{
    internal class ApplicationRepository : RepositoryBase, IApplicationRepository
    {
        public ApplicationRepository(IDataContextManager dataContextManager)
            : base(dataContextManager)
        {
        }

        public bool GetIsEnabled()
        {
            return DataContext.IsApplicationEnabled;
        }

        public bool IsFirstTimeUserExperienceCompleted()
        {
            return DataContext.IsFirstTimeUserExperienceCompleted;
        }

        public void UpdateFirstTimeUserExperience(bool isCompleted)
        {
            DataContext.IsFirstTimeUserExperienceCompleted = isCompleted;
        }

        public void UpdateIsEnabled(bool isEnabled)
        {
            DataContext.IsApplicationEnabled = isEnabled;
        }
    }
}
