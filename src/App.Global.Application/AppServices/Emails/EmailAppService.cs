﻿using App.Global.AppServiceDefines.Emails;
using App.Global.Commons.GenericApis;
using App.Global.Commons.Helpers;
using App.Global.DataTranferObjects.Emails;
using App.Global.Entitis.Emails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
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
        private readonly EmailHelper _emailHelper;

        public EmailAppService(
            IRepository<Service_SendMail> emailRepository,
            IRepository<EmailTemplate> templateRepository,
            IdentityUserManager userManager,
            EmailHelper emailHelper)
        {
            _emailRepository = emailRepository;
            _templateRepository = templateRepository;
            _userManager = userManager;
            _emailHelper = emailHelper;
        }

        public async Task<IActionResult> CreateAsync(Service_SendMailDto input)
        {
            var email = ObjectMapper.Map<Service_SendMailDto, Service_SendMail>(input);
            email.Status = (int)EmailStatusEnum.Processing;
            email = await _emailRepository.InsertAsync(email, true);
            await CurrentUnitOfWork.SaveChangesAsync();
            await _emailHelper.SendMailAsync(email.Id);
            return new OkObjectResult(new GenericActionResult()
            {
                StatusCode = 200,
                Success = true,
                Message = "Send Email successfully!"
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
                if (user != null)
                    result.CreateUserName = user.Name;
            }
            return result;
        }

        public async Task<PagedResultDto<Service_SendMailDto>> GetListAsync(GenericFilterInput input)
        {
            if (string.IsNullOrEmpty(input.Sorting))
                input.Sorting = nameof(Service_SendMail.CreationTime) + " descending";

            var emails = (await _emailRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrEmpty(input.Filter), x => x.ReceiverEmail.ToLower().Contains(input.Filter.ToLower()));
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

            email.Status = (int)EmailStatusEnum.Processing;
            await CurrentUnitOfWork.SaveChangesAsync();
            await _emailHelper.SendMailAsync(email.Id);
            return new OkObjectResult(new GenericActionResult()
            {
                StatusCode = 200,
                Success = true,
                Message = "Send Email successfully!"
            });
        }
    }
}
