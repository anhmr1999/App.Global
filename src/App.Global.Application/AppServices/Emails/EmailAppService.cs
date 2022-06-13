using App.Global.AppServiceDefines.Emails;
using App.Global.Commons.GenericApis;
using App.Global.Commons.Helpers;
using App.Global.DataTranferObjects.Emails;
using App.Global.Entitis.Emails;
using App.Global.EventHanlderModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace App.Global.AppServices.Emails
{
    [Authorize]
    public class EmailAppService : GlobalAppService, IEmailAppService
    {
        private readonly IRepository<Service_SendMail> _emailRepository;
        private readonly IRepository<EmailTemplate> _templateRepository;
        private readonly IdentityUserManager _userManager;
        private readonly IBackgroundJobManager _backgroundJobManager;

        public EmailAppService(
            IRepository<Service_SendMail> emailRepository,
            IRepository<EmailTemplate> templateRepository,
            IdentityUserManager userManager,
            IBackgroundJobManager backgroundJobManager)
        {
            _emailRepository = emailRepository;
            _templateRepository = templateRepository;
            _userManager = userManager;
            _backgroundJobManager = backgroundJobManager;
        }

        public async Task<IActionResult> CreateAsync(Service_SendMailDto input)
        {
            var email = ObjectMapper.Map<Service_SendMailDto, Service_SendMail>(input);
            email.Status = (int)EmailStatusEnum.Created;
            email = await _emailRepository.InsertAsync(email, true);
            await CurrentUnitOfWork.SaveChangesAsync();
            await _backgroundJobManager.EnqueueAsync(new EmailSendingArgs() { EmailId = email.Id });
            return new OkObjectResult(new GenericActionResult()
            {
                StatusCode = 200,
                Success = true,
                Message = "Email is being sent!"
            });
        }

        public async Task<Service_SendMailDto> GetAsync(Guid id)
        {
            var email = await _emailRepository.FindAsync(x => x.Id == id);
            var result = ObjectMapper.Map<Service_SendMail, Service_SendMailDto>(email);
            var template = await _templateRepository.FirstOrDefaultAsync(x => x.Id == email.TemplateId);
            if (template != null)
                result.TemplateDto = ObjectMapper.Map<EmailTemplate, EmailTemplateDto>(template);
            if (email.CreatorId.HasValue)
            {
                var user = await _userManager.GetByIdAsync(email.CreatorId.Value);
                result.CreateUserName = user?.Name;
            }
            return result;
        }

        public async Task<PagedResultDto<Service_SendMailDto>> GetListAsync(GenericFilterInput input)
        {
            if (string.IsNullOrEmpty(input.Sorting))
                input.Sorting = nameof(Service_SendMail.CreationTime) + " descending";

            var emails = (await _emailRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrEmpty(input.Filter), x => x.ReceiverEmail.ToLower().Contains(input.Filter.ToLower()))
                .WhereIf(input.Status.HasValue, x => x.Status == input.Status);
            var totalCount = emails.Count();
            var lstEmail = emails.OrderBy(input.Sorting).PageBy(input).ToList();
            var resultLst = ObjectMapper.Map<List<Service_SendMail>, List<Service_SendMailDto>>(lstEmail);
            var lstTemplate = await _templateRepository.GetListAsync();
            foreach (var item in resultLst)
            {
                var template = lstTemplate.FirstOrDefault(x => x.Id == item.TemplateId);
                if (template != null)
                    item.TemplateDto = ObjectMapper.Map<EmailTemplate, EmailTemplateDto>(template);
            }
            return new PagedResultDto<Service_SendMailDto>(totalCount, resultLst);
        }

        [Route("re-send/{id}")]
        public async Task<IActionResult> ReSendAsync(Guid id)
        {
            var email = await _emailRepository.GetAsync(x => x.Id == id);

            email.Status = (int)EmailStatusEnum.ReSended;
            await CurrentUnitOfWork.SaveChangesAsync();
            await _backgroundJobManager.EnqueueAsync(new EmailSendingArgs() { EmailId = email.Id });
            return new OkObjectResult(new GenericActionResult()
            {
                StatusCode = 200,
                Success = true,
                Message = "Email is being sent!"
            });
        }
    }
}
