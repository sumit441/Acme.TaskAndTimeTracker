using Volo.Abp.Modularity;

namespace Acme.TaskAndTimeTracker;

public abstract class TaskAndTimeTrackerApplicationTestBase<TStartupModule> : TaskAndTimeTrackerTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
