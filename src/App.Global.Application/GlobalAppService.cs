using System;
using System.Collections.Generic;
using System.Text;
using App.Global.Localization;
using Volo.Abp.Application.Services;

namespace App.Global;

/* Inherit your application services from this class.
 */
public abstract class GlobalAppService : ApplicationService
{
    protected GlobalAppService()
    {
        LocalizationResource = typeof(GlobalResource);
    }
}
