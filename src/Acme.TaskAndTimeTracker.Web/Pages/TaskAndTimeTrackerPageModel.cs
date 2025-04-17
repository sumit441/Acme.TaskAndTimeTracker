using Acme.TaskAndTimeTracker.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Acme.TaskAndTimeTracker.Web.Pages;

public abstract class TaskAndTimeTrackerPageModel : AbpPageModel
{
    protected TaskAndTimeTrackerPageModel()
    {
        LocalizationResourceType = typeof(TaskAndTimeTrackerResource);
    }
}
