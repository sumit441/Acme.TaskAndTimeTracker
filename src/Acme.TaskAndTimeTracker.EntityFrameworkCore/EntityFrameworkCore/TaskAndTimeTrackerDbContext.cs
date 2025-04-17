using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Acme.TaskAndTimeTracker.ApplicationWorkflowInstances;

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
    public DbSet<ApplicationWorkflowInstance> ApplicationWorkflowInstances { get; set; }

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

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(TaskAndTimeTrackerConsts.DbTablePrefix + "YourEntities", TaskAndTimeTrackerConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
        builder.Entity<ApplicationWorkflowInstance>(b =>
        {
            b.ToTable("ApplicationWorkflowInstances"); // Table name
            b.HasKey(x => x.Id);

            b.Property(x => x.TenantId).IsRequired(false);
            b.Property(x => x.OperationWorkflowInstanceId).IsRequired();
            b.Property(x => x.OperationWorkflowConfigurationId).IsRequired();
            b.Property(x => x.InstanceId).HasMaxLength(200).IsRequired(false);
            b.Property(x => x.InitialData).IsRequired();
            b.Property(x => x.IntermediateData).IsRequired(false);
            b.Property(x => x.WorkflowStageId).IsRequired();
            b.Property(x => x.WorkflowSubStageId).IsRequired(false);
            b.Property(x => x.Remarks).IsRequired(false);
            b.Property(x => x.WorkflowStageDate).IsRequired();
            b.Property(x => x.RequestedBy).HasMaxLength(200).IsRequired(false);

            // Indexes (if needed)
            b.HasIndex(x => x.OperationWorkflowInstanceId);
            b.HasIndex(x => x.WorkflowStageId);
        });
    }
}
