using Acme.TaskAndTimeTracker.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Acme.TaskAndTimeTracker.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(TaskAndTimeTrackerEntityFrameworkCoreModule),
    typeof(TaskAndTimeTrackerApplicationContractsModule)
)]
public class TaskAndTimeTrackerDbMigratorModule : AbpModule
{
}
