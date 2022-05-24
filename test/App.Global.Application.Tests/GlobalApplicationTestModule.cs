using Volo.Abp.Modularity;

namespace App.Global;

[DependsOn(
    typeof(GlobalApplicationModule),
    typeof(GlobalDomainTestModule)
    )]
public class GlobalApplicationTestModule : AbpModule
{

}
