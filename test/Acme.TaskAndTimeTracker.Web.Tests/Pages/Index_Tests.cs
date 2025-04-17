using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Acme.TaskAndTimeTracker.Pages;

[Collection(TaskAndTimeTrackerTestConsts.CollectionDefinitionName)]
public class Index_Tests : TaskAndTimeTrackerWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
