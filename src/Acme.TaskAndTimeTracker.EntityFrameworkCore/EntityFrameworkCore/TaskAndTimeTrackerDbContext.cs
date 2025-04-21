using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Acme.TaskAndTimeTracker.Projects;
using Acme.TaskAndTimeTracker.TimeEntries;
using Acme.TaskAndTimeTracker.Tasks;

namespace Acme.TaskAndTimeTracker.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class TaskAndTimeTrackerDbContext :
    AbpDbContext<TaskAndTimeTrackerDbContext>,
    ITenantManagementDbContext,
    IIdentityDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */


    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext and ISaasDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext and ISaasDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectTask> ProjectTasks { get; set; }
    public DbSet<TimeEntry> TimeEntries { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public TaskAndTimeTrackerDbContext(DbContextOptions<TaskAndTimeTrackerDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureTenantManagement();
        builder.ConfigureBlobStoring();

        builder.Entity<Project>(b =>
        {
            b.ToTable("Projects");
            b.HasKey(x => x.Id);
            b.Property(x => x.Name).IsRequired().HasMaxLength(200);
            b.Property(x => x.Description).HasMaxLength(1000);

            // If soft delete is implemented, handle query filters for soft-deleted entities
            b.HasQueryFilter(x => !x.IsDeleted); // Assuming "IsDeleted" is your soft-delete field
        });

        builder.Entity<ProjectTask>(b =>
        {
            b.ToTable("ProjectTasks");
            b.HasKey(x => x.Id);
            b.Property(x => x.Title).IsRequired().HasMaxLength(128);
            b.Property(x => x.Description).HasMaxLength(1024);
            b.Property(x => x.DueDate).IsRequired();

            b.Property(x => x.Status)
                .IsRequired()
                .HasConversion<int>();

            b.HasOne(x => x.Project)
                 .WithMany(p => p.ProjectTasks) // ✅ Navigation property on Project entity
                 .HasForeignKey(x => x.ProjectId)
                 .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.AssignedUser) // Each ProjectTask is assigned to one user
                .WithMany() // User can be assigned many tasks (user side will use IdentityUser)
                .HasForeignKey(x => x.AssignedUserId) // Foreign key on AssignedUserId
                .OnDelete(DeleteBehavior.SetNull); // When the user is deleted, set the AssignedUserId to null

            // Ensure the ProjectTask does not get affected by the soft-delete filter
            b.HasQueryFilter(x => x.Project != null && !x.Project.IsDeleted); // Ensure Project is not deleted
        });

        builder.Entity<TimeEntry>(b =>
        {
            b.ToTable("TimeEntries");
            b.HasKey(x => x.Id);
            b.Property(x => x.LoggedHours).IsRequired();
            b.Property(x => x.LogDate).IsRequired();
            b.Property(x => x.Notes).HasMaxLength(1000);

            // Explicitly define the relationship with ProjectTask
            b.HasOne(x => x.Task) // Use the navigation property in TimeEntry
                .WithMany() // No navigation property in ProjectTask
                .HasForeignKey(x => x.TaskId) // Use TaskId as the foreign key
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete when ProjectTask is deleted
        });
    }
}
