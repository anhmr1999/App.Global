using App.Global.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace App.Global.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class GlobalController : AbpControllerBase
{
    protected GlobalController()
    {
        LocalizationResource = typeof(GlobalResource);
    }
}
