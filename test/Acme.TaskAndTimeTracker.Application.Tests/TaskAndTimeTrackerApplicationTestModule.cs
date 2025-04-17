using Volo.Abp.Modularity;

namespace Acme.TaskAndTimeTracker;

[DependsOn(
    typeof(TaskAndTimeTrackerApplicationModule),
    typeof(TaskAndTimeTrackerDomainTestModule)
)]
public class TaskAndTimeTrackerApplicationTestModule : AbpModule
{

}
