using Xunit;

namespace Acme.TaskAndTimeTracker.EntityFrameworkCore;

[CollectionDefinition(TaskAndTimeTrackerTestConsts.CollectionDefinitionName)]
public class TaskAndTimeTrackerEntityFrameworkCoreCollection : ICollectionFixture<TaskAndTimeTrackerEntityFrameworkCoreFixture>
{

}
