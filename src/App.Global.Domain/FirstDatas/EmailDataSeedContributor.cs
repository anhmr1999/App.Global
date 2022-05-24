using App.Global.Entitis.Emails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace App.Global.FirstDatas
{
    internal class EmailDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<EmailConfig> _configRepository;

        public EmailDataSeedContributor(IRepository<EmailConfig> configRepository)
        {
            _configRepository = configRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (!await _configRepository.AnyAsync())
            {
                await _configRepository.InsertManyAsync(new List<EmailConfig>() {
                    new EmailConfig(){ Code = "MAIL_ACCOUNT", Value = "mr.anhnh1999@gmail.com" , Description = "Email"},
                    new EmailConfig(){ Code = "MAIL_PASSWORD", Value = "123456" , Description = "Password"},
                    new EmailConfig(){ Code = "MAIL_NAME", Value = "App.Global" , Description = "Password"},
                    new EmailConfig(){ Code = "MAIL_FROM", Value = "App.Global" , Description = "Password"},
                    new EmailConfig(){ Code = "MAIL_SMTP", Value = "smtp.gmail.com" , Description = "Password"},
                    new EmailConfig(){ Code = "MAIL_PORT", Value = "587" , Description = "Password"},
                    new EmailConfig(){ Code = "MAIL_SSL", Value = "true" , Description = "Password"}
                });
            }
        }
    }
}
