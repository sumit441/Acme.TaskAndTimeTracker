namespace Acme.TaskAndTimeTracker.Permissions;

public static class TaskAndTimeTrackerPermissions
{
    public const string GroupName = "TaskAndTimeTracker";

    public static class Projects
    {
        public const string Default = GroupName + ".Projects";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class ProjectTasks
    {
        public const string Default = GroupName + ".ProjectTasks";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class TimeEntries
    {
        public const string Default = GroupName + ".TimeEntries";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class Reports
    {
        public const string Generate = GroupName + ".Reports.Generate";
    }
}
