using App.Global.AppServiceDefines.Emails;
using App.Global.DataTranferObjects.Emails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace App.Global.Web.Pages.Mails
{
    public class ConfigModalModel : GlobalPageModel
    {
        public ICollection<EmailConfigDto> Configs { get; set; }
        private readonly IEmailConfigAppService _configAppService;

        public ConfigModalModel(IEmailConfigAppService configAppService)
        {
            _configAppService = configAppService;
        }
        public void OnGet()
        {
            Configs = _configAppService.GetListAsync().Result;
        }
    }
}
