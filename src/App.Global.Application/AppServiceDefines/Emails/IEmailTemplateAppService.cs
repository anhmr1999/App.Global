using App.Global.Commons.GenericApis;
using App.Global.DataTranferObjects.Emails;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace App.Global.AppServiceDefines.Emails
{
    public interface IEmailTemplateAppService : IApplicationService
    {
        Task<PagedResultDto<EmailTemplateDto>> GetListAsync(GenericFilterInput input);
        Task<List<EmailTemplateDto>> GetAllAsync();
        Task<EmailTemplateDto> GetAsync(Guid id);
        Task<IActionResult> CreateAsync(EmailTemplateDto input);
        Task<IActionResult> UpdateAsync(EmailTemplateDto input);
        Task<IActionResult> DeleteAsync(Guid id);
    }
}
