using App.Global.AppServiceDefines.Emails;
using App.Global.DataTranferObjects.Emails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.SettingManagement;

namespace App.Global.Web.Pages.Mails
{
    public class ConfigModalModel : GlobalPageModel
    {
        [BindProperty]
        public EmailSettingsDto Settings { get; set; }
        private readonly IEmailSettingsAppService _configAppService;

        public ConfigModalModel(IEmailSettingsAppService configAppService)
        {
            _configAppService = configAppService;
        }
        public async Task OnGetAsync()
        {
            Settings = await _configAppService.GetAsync();
        }

        public async Task OnPostAsync()
        {
            var updateSetting = ObjectMapper.Map<EmailSettingsDto, UpdateEmailSettingsDto>(Settings);
            await _configAppService.UpdateAsync(updateSetting);
        }
    }
}
