using Volo.Abp.Modularity;

namespace Acme.TaskAndTimeTracker;

/* Inherit from this class for your domain layer tests. */
public abstract class TaskAndTimeTrackerDomainTestBase<TStartupModule> : TaskAndTimeTrackerTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
