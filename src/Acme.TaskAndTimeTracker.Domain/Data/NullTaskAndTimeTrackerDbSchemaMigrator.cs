using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Acme.TaskAndTimeTracker.Data;

/* This is used if database provider does't define
 * ITaskAndTimeTrackerDbSchemaMigrator implementation.
 */
public class NullTaskAndTimeTrackerDbSchemaMigrator : ITaskAndTimeTrackerDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
