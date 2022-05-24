using App.Global.AppServiceDefines.Emails;
using App.Global.Commons.GenericApis;
using App.Global.DataTranferObjects.Emails;
using App.Global.Entitis.Emails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace App.Global.AppServices.Emails
{
    [Authorize]
    public class EmailConfigAppService : GlobalAppService, IEmailConfigAppService
    {
        private readonly IRepository<EmailConfig> _configRepository;

        public EmailConfigAppService(IRepository<EmailConfig> configRepository)
        {
            _configRepository = configRepository;
        }

        public async Task<IEnumerable<EmailConfigDto>> GetListAsync()
        {
            var lst = await _configRepository.GetListAsync();
            return ObjectMapper.Map<List<EmailConfig>, List<EmailConfigDto>>(lst);
        }

        public async Task<IActionResult> UpdateAsync(EmailConfigRequestDto input)
        {
            var configs = await _configRepository.GetListAsync();
            var newConfigs = new List<EmailConfig>();
            foreach (var item in input.Configs)
            {
                var config = configs.FirstOrDefault(x => x.Code == item.Code);
                config.Value = item.Value;
                newConfigs.Add(config);
            }
            await _configRepository.UpdateManyAsync(newConfigs);
            return new OkObjectResult(new GenericActionResult { Success = true, StatusCode = 200 });
        }
    }
}
