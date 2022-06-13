using App.Global.AppServiceDefines.Emails;
using App.Global.DataTranferObjects.Emails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace App.Global.Web.Pages.Mails
{
    public class EmailTemplateModalModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        public EmailTemplateDto TemplateDto { get; set; }

        private readonly IEmailTemplateAppService _templateAppService;
        public EmailTemplateModalModel(IEmailTemplateAppService templateAppService)
        {
            _templateAppService = templateAppService;
        }

        public async Task OnGet()
        {
            TemplateDto = await _templateAppService.GetAsync(Id);
        }
    }
}
