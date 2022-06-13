using App.Global.AppServiceDefines.Emails;
using App.Global.DataTranferObjects.Emails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Global.Web.Pages.Mails
{
    public class CreateEmailModalModel : PageModel
    {
        public List<EmailTemplateDto> Templates { get; set; }
        private readonly IEmailTemplateAppService _templateService;

        public CreateEmailModalModel(IEmailTemplateAppService templateService)
        {
            _templateService = templateService;
        }

        public async Task OnGet()
        {
            Templates = await _templateService.GetAllAsync();
        }
    }
}
