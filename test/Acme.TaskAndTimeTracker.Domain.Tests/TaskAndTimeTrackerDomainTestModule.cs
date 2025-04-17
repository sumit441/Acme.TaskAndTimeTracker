using Volo.Abp.Modularity;

namespace Acme.TaskAndTimeTracker;

[DependsOn(
    typeof(TaskAndTimeTrackerDomainModule),
    typeof(TaskAndTimeTrackerTestBaseModule)
)]
public class TaskAndTimeTrackerDomainTestModule : AbpModule
{

}
