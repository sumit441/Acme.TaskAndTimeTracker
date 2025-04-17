using Volo.Abp.Settings;

namespace Acme.TaskAndTimeTracker.Settings;

public class TaskAndTimeTrackerSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(TaskAndTimeTrackerSettings.MySetting1));
    }
}
