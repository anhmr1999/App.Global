using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.Emailing;
using Volo.Abp.BackgroundJobs;

namespace App.Global;

[DependsOn(
    typeof(GlobalDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(GlobalApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpBackgroundJobsModule)
    )]
[DependsOn(typeof(AbpEmailingModule))]
    public class GlobalApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<GlobalApplicationModule>();
        });
        Configure<AbpBackgroundJobOptions>(options =>
        {
            //options.IsJobExecutionEnabled = false; //Disables job execution
        });
    }
}
