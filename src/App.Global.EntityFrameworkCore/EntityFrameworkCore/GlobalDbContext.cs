using App.Global.Entitis.Emails;
using App.Global.Entitis.ExcelServices;
using App.Global.Entitis.Organizations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace App.Global.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class GlobalDbContext :
    AbpDbContext<GlobalDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }
    public DbSet<Province> Provinces { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Commune> Communes { get; set; }
    public DbSet<EmailTemplate> EmailTemplates { get; set; }
    public DbSet<Service_SendMail> Service_SendMail { get; set; }
    public DbSet<ExcelService> ExcelServices { get; set; }
    #endregion

    public GlobalDbContext(DbContextOptions<GlobalDbContext> options)
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
        builder.ConfigureIdentity();
        builder.ConfigureIdentityServer();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(GlobalConsts.DbTablePrefix + "YourEntities", GlobalConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
        builder.Entity<Province>(b => { b.ToTable(GlobalConsts.DbTablePrefix + "Provinces", GlobalConsts.DbSchema); });
        builder.Entity<District>(b => { b.ToTable(GlobalConsts.DbTablePrefix + "Districts", GlobalConsts.DbSchema); });
        builder.Entity<Commune>(b => { b.ToTable(GlobalConsts.DbTablePrefix + "Communes", GlobalConsts.DbSchema); });
        builder.Entity<EmailTemplate>(b => { b.ToTable(GlobalConsts.DbTablePrefix + "EmailTemplates", GlobalConsts.DbSchema); });
        builder.Entity<Service_SendMail>(b => { b.ToTable(GlobalConsts.DbTablePrefix + "Service_SendMails", GlobalConsts.DbSchema); });
        builder.Entity<ExcelService>(b => { b.ToTable(GlobalConsts.DbTablePrefix + "ExcelServices", GlobalConsts.DbSchema); });
    }
}
