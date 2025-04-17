using Acme.TaskAndTimeTracker.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Acme.TaskAndTimeTracker.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class TaskAndTimeTrackerController : AbpControllerBase
{
    protected TaskAndTimeTrackerController()
    {
        LocalizationResource = typeof(TaskAndTimeTrackerResource);
    }
}
