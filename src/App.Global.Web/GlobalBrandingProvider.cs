using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace App.Global.Web;

[Dependency(ReplaceServices = true)]
public class GlobalBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Global";
}
