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

namespace App.Global.AppServices.Emails
{
    [Authorize]
    public class EmailAppService : GlobalAppService, IEmailAppService
    {
        private readonly IRepository<Service_SendMail> _emailRepository;
        private readonly IRepository<EmailTemplate> _templateRepository;
        private readonly EmailHelper _emailHelper;

        public EmailAppService(
            IRepository<Service_SendMail> emailRepository,
            IRepository<EmailTemplate> templateRepository,
            EmailHelper emailHelper)
        {
            _emailRepository = emailRepository;
            _templateRepository = templateRepository;
            _emailHelper = emailHelper;
        }

        public async Task<IActionResult> CreateAsync(Service_SendMailDto input)
        {
            var email = ObjectMapper.Map<Service_SendMailDto, Service_SendMail>(input);
            email.Status = (int)EmailStatusEnum.Created;
            email = await _emailRepository.InsertAsync(email, true);
            await CurrentUnitOfWork.SaveChangesAsync();
            await _emailHelper.SendEmail(new EmailInput() { EmailId = email.Id });
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
            return ObjectMapper.Map<Service_SendMail, Service_SendMailDto>(email);
        }

        public async Task<PagedResultDto<Service_SendMailDto>> GetListAsync(GenericFilterInput input)
        {
            if (string.IsNullOrEmpty(input.Sorting))
                input.Sorting = nameof(Service_SendMail.CreationTime) + " descending";

            var emails = (await _emailRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrEmpty(input.Filter), x => x.ReceiverEmail.ToLower().Contains(input.Filter.ToLower()));
            var totalCount = emails.Count();
            var lstEmailDto = emails.OrderBy(input.Sorting).PageBy(input).ToList();
            var resultLst = ObjectMapper.Map<List<Service_SendMail>, List<Service_SendMailDto>>(lstEmailDto);
            var lstTemplate = await _templateRepository.GetListAsync(x => x.IsActive);
            foreach (var item in resultLst)
            {
                var template = lstTemplate.FirstOrDefault(x => x.Id == item.TemplateId);
                if (template != null)
                    item.TemplateDto = ObjectMapper.Map<EmailTemplate, EmailTemplateDto>(template);
            }
            return new PagedResultDto<Service_SendMailDto>(totalCount, resultLst);
        }
    }
}