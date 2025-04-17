using Acme.TaskAndTimeTracker.Samples;
using Xunit;

namespace Acme.TaskAndTimeTracker.EntityFrameworkCore.Domains;

[Collection(TaskAndTimeTrackerTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<TaskAndTimeTrackerEntityFrameworkCoreTestModule>
{

}
