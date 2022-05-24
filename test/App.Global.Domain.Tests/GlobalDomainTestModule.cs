using App.Global.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace App.Global;

[DependsOn(
    typeof(GlobalEntityFrameworkCoreTestModule)
    )]
public class GlobalDomainTestModule : AbpModule
{

}
