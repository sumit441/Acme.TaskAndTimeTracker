using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Localization;
using Acme.TaskAndTimeTracker.Localization;

namespace Acme.TaskAndTimeTracker.Web;

[Dependency(ReplaceServices = true)]
public class TaskAndTimeTrackerBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<TaskAndTimeTrackerResource> _localizer;

    public TaskAndTimeTrackerBrandingProvider(IStringLocalizer<TaskAndTimeTrackerResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
