using System.Threading.Tasks;

namespace Acme.TaskAndTimeTracker.Data;

public interface ITaskAndTimeTrackerDbSchemaMigrator
{
    Task MigrateAsync();
}
