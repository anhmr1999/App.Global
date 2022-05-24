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
    public interface IEmailConfigAppService : IApplicationService
    {
        Task<IEnumerable<EmailConfigDto>> GetListAsync();
        Task<IActionResult> UpdateAsync(EmailConfigRequestDto input);
    }
}
