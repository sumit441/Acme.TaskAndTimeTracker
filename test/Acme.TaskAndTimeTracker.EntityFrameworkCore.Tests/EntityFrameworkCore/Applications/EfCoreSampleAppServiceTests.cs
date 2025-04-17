using Acme.TaskAndTimeTracker.Samples;
using Xunit;

namespace Acme.TaskAndTimeTracker.EntityFrameworkCore.Applications;

[Collection(TaskAndTimeTrackerTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<TaskAndTimeTrackerEntityFrameworkCoreTestModule>
{

}
