using App.Global.AppServiceDefines.Emails;
using App.Global.DataTranferObjects.Emails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace App.Global.Web.Pages.Mails
{
    public class EmailModalModel : GlobalPageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        public Service_SendMailDto Email { get; set; }

        private readonly IEmailAppService _emailAppService;

        public EmailModalModel(IEmailAppService emailAppService)
        {
            _emailAppService = emailAppService;
        }

        public async Task OnGet()
        {
            Email = await _emailAppService.GetAsync(Id);
        }
    }
}
