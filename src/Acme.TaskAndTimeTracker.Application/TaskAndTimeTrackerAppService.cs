using Acme.TaskAndTimeTracker.Localization;
using Volo.Abp.Application.Services;

namespace Acme.TaskAndTimeTracker;

/* Inherit your application services from this class.
 */
public abstract class TaskAndTimeTrackerAppService : ApplicationService
{
    protected TaskAndTimeTrackerAppService()
    {
        LocalizationResource = typeof(TaskAndTimeTrackerResource);
    }
}
