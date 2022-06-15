using App.Global.Commons.GenericApis;
using App.Global.DataTranferObjects.Emails;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace App.Global.AppServiceDefines.Emails
{
    public interface IEmailAppService : IApplicationService
    {
        Task<PagedResultDto<Service_SendMailDto>> GetListAsync(GenericFilterInput input);
        Task<Service_SendMailDto> GetAsync(Guid id);
        Task<IActionResult> CreateAsync(Service_SendMailDto input);
        Task<IActionResult> ReSendAsync(Guid id);
        Task<IActionResult> GetExportFile(GenericFilterInput input);
    }
}
