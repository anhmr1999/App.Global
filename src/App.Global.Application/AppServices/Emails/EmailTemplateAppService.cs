using App.Global.AppServiceDefines.Emails;
using App.Global.Commons.GenericApis;
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
    public class EmailTemplateAppService : GlobalAppService, IEmailTemplateAppService
    {
        private readonly IRepository<EmailTemplate> _templateRepository;

        public EmailTemplateAppService(IRepository<EmailTemplate> templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public async Task<IActionResult> CreateAsync(EmailTemplateDto input)
        {
            var template = ObjectMapper.Map<EmailTemplateDto, EmailTemplate>(input);
            template = await _templateRepository.InsertAsync(template);
            return new OkObjectResult(new GenericActionResult()
            {
                StatusCode = 200,
                Success = true,
                Data = ObjectMapper.Map<EmailTemplate, EmailTemplateDto>(template)
            });
        }

        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _templateRepository.DeleteAsync(x => x.Id == id);
            return new OkObjectResult(new GenericActionResult() { StatusCode = 200, Success = true});
        }

        [Route("api/app/email-template/all")]
        public async Task<List<EmailTemplateDto>> GetAllAsync()
        {
            var lst = await _templateRepository.GetListAsync(x => x.IsActive);
            return ObjectMapper.Map<List<EmailTemplate>, List<EmailTemplateDto>>(lst);
        }

        public async Task<EmailTemplateDto> GetAsync(Guid id)
        {
            var template = await _templateRepository.FindAsync(x => x.Id == id);
            return ObjectMapper.Map<EmailTemplate, EmailTemplateDto>(template);
        }

        public async Task<PagedResultDto<EmailTemplateDto>> GetListAsync(GenericFilterInput input)
        {
            if (string.IsNullOrEmpty(input.Sorting))
                input.Sorting = nameof(EmailTemplate.CreationTime) + " descending";

            var lst = (await _templateRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrEmpty(input.Filter), x => 
                    x.DefaultTitle.ToLower().Contains(input.Filter.ToLower()) ||
                    x.TemplateName.ToLower().Contains(input.Filter.ToLower()));
            var totalCount = lst.Count();
            var templates = lst.OrderBy(input.Sorting).PageBy(input).ToList();
            return new PagedResultDto<EmailTemplateDto>(totalCount, ObjectMapper.Map<List<EmailTemplate>, List<EmailTemplateDto>>(templates));
        }

        public async Task<IActionResult> UpdateAsync(EmailTemplateDto input)
        {
            var template = await _templateRepository.FindAsync(x => x.Id == input.Id.Value);
            ObjectMapper.Map(input, template);
            await _templateRepository.UpdateAsync(template);
            return new OkObjectResult(new GenericActionResult()
            {
                StatusCode = 200,
                Success = true,
                Data = ObjectMapper.Map<EmailTemplate, EmailTemplateDto>(template)
            });
        }
    }
}
