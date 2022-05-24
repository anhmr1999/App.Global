using App.Global.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace App.Global.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(GlobalEntityFrameworkCoreModule),
    typeof(GlobalApplicationContractsModule)
    )]
public class GlobalDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
    }
}
