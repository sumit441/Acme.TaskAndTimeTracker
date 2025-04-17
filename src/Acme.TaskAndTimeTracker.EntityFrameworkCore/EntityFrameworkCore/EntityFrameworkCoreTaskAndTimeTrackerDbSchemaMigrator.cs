using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Acme.TaskAndTimeTracker.Data;
using Volo.Abp.DependencyInjection;

namespace Acme.TaskAndTimeTracker.EntityFrameworkCore;

public class EntityFrameworkCoreTaskAndTimeTrackerDbSchemaMigrator
    : ITaskAndTimeTrackerDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreTaskAndTimeTrackerDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the TaskAndTimeTrackerDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<TaskAndTimeTrackerDbContext>()
            .Database
            .MigrateAsync();
    }
}
