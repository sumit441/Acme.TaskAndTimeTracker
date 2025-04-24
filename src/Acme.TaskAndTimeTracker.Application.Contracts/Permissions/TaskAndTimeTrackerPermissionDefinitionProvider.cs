using Acme.TaskAndTimeTracker.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Acme.TaskAndTimeTracker.Permissions;

public class TaskAndTimeTrackerPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var taskAndTimeTrackerGroup = context.AddGroup(TaskAndTimeTrackerPermissions.GroupName, L("Permission:TaskAndTimeTracker"));

        // Permissions for Project
        var projectPermission = taskAndTimeTrackerGroup.AddPermission(TaskAndTimeTrackerPermissions.Projects.Default, L("Permission:Projects"));
        projectPermission.AddChild(TaskAndTimeTrackerPermissions.Projects.Create, L("Permission:Create"));
        projectPermission.AddChild(TaskAndTimeTrackerPermissions.Projects.Update, L("Permission:Update"));
        projectPermission.AddChild(TaskAndTimeTrackerPermissions.Projects.Delete, L("Permission:Delete"));

        // Permissions for ProjectTask
        var projectTaskPermission = taskAndTimeTrackerGroup.AddPermission(TaskAndTimeTrackerPermissions.ProjectTasks.Default, L("Permission:ProjectTasks"));
        projectTaskPermission.AddChild(TaskAndTimeTrackerPermissions.ProjectTasks.Create, L("Permission:Create"));
        projectTaskPermission.AddChild(TaskAndTimeTrackerPermissions.ProjectTasks.Update, L("Permission:Update"));
        projectTaskPermission.AddChild(TaskAndTimeTrackerPermissions.ProjectTasks.Delete, L("Permission:Delete"));

        // Permissions for TimeEntry
        var timeEntryPermission = taskAndTimeTrackerGroup.AddPermission(TaskAndTimeTrackerPermissions.TimeEntries.Default, L("Permission:TimeEntries"));
        timeEntryPermission.AddChild(TaskAndTimeTrackerPermissions.TimeEntries.Create, L("Permission:Create"));
        timeEntryPermission.AddChild(TaskAndTimeTrackerPermissions.TimeEntries.Update, L("Permission:Update"));
        timeEntryPermission.AddChild(TaskAndTimeTrackerPermissions.TimeEntries.Delete, L("Permission:Delete"));

        var reportsPermission = taskAndTimeTrackerGroup.AddPermission(TaskAndTimeTrackerPermissions.Reports.Default, L("Permission:Reports"));
        reportsPermission.AddChild(TaskAndTimeTrackerPermissions.Reports.ViewByProject, L("Permission:ViewByProject"));
        reportsPermission.AddChild(TaskAndTimeTrackerPermissions.Reports.ViewByUser, L("Permission:ViewByUser"));
        reportsPermission.AddChild(TaskAndTimeTrackerPermissions.Reports.ViewByTask, L("Permission:ViewByTask"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<TaskAndTimeTrackerResource>(name);
    }
}
