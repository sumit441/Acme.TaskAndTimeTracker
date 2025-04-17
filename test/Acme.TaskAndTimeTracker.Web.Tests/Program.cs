using Microsoft.AspNetCore.Builder;
using Acme.TaskAndTimeTracker;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("Acme.TaskAndTimeTracker.Web.csproj"); 
await builder.RunAbpModuleAsync<TaskAndTimeTrackerWebTestModule>(applicationName: "Acme.TaskAndTimeTracker.Web");

public partial class Program
{
}
