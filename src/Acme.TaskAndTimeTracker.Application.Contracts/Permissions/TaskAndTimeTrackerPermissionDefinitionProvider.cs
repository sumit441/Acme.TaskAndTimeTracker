using Acme.TaskAndTimeTracker.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Acme.TaskAndTimeTracker.Permissions;

public class TaskAndTimeTrackerPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(TaskAndTimeTrackerPermissions.GroupName);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(TaskAndTimeTrackerPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<TaskAndTimeTrackerResource>(name);
    }
}
