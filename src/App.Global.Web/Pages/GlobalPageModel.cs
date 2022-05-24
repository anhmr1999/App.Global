using App.Global.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace App.Global.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class GlobalPageModel : AbpPageModel
{
    protected GlobalPageModel()
    {
        LocalizationResourceType = typeof(GlobalResource);
    }
}
