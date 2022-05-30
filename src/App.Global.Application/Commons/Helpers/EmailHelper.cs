using App.Global.Entitis.Emails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Emailing;
using Volo.Abp.SettingManagement;

namespace App.Global.Commons.Helpers
{
    public class EmailHelper : ITransientDependency
    {
        private readonly IRepository<Service_SendMail> _emailRepository;
        private readonly IRepository<EmailTemplate> _templateRepository;
        private readonly IEmailSettingsAppService _settingsAppService;

        public EmailHelper(
            IRepository<Service_SendMail> emailRepository,
            IRepository<EmailTemplate> templateRepository,
            IEmailSettingsAppService settingsAppService)
        {
            _emailRepository = emailRepository;
            _templateRepository = templateRepository;
            _settingsAppService = settingsAppService;
        }

        public async Task SendMailAsync(Guid EmailId)
        {
            var serviceMail = await _emailRepository.GetAsync(x => x.Id == EmailId);
            var Settings = await _settingsAppService.GetAsync();
            if (serviceMail != null && Settings != null)
            {
                var subject = Settings.DefaultFromDisplayName;
                if (serviceMail.TemplateId.HasValue)
                    subject = (await _templateRepository.GetAsync(x => x.Id == EmailId)).DefaultTitle;
                using (SmtpClient client = new SmtpClient(Settings.SmtpHost))
                {
                    client.Port = Settings.SmtpPort;
                    client.Credentials = new NetworkCredential(Settings.SmtpUserName, Settings.SmtpPassword);
                    client.EnableSsl = Settings.SmtpEnableSsl;
                    MailMessage message = new MailMessage(
                        from: Settings.SmtpUserName,
                        to: serviceMail.ReceiverEmail,
                        subject: subject,
                        body: await GetEmailContent(serviceMail)
                    );

                    message.BodyEncoding = Encoding.UTF8;
                    message.SubjectEncoding = Encoding.UTF8;
                    message.IsBodyHtml = true;
                    message.ReplyToList.Add(new MailAddress(Settings.SmtpUserName));
                    message.Sender = new MailAddress(Settings.SmtpUserName);
                    try
                    {
                        await client.SendMailAsync(message);
                        serviceMail.NumberOfTimeSend += 1;
                        serviceMail.Status = (int)EmailStatusEnum.Done;
                        await _emailRepository.UpdateAsync(serviceMail);
                    }
                    catch (Exception)
                    {
                        serviceMail.Status = (int)EmailStatusEnum.Fail;
                        await _emailRepository.UpdateAsync(serviceMail);
                        throw;
                    }
                }
            }    
        }

        public async Task<string> GetEmailContent(Service_SendMail serviceMail)
        {
            var content = new StringBuilder();
            if (!string.IsNullOrEmpty(serviceMail.Content))
                return serviceMail.Content;
            if (!serviceMail.TemplateId.HasValue)
                return null;
            var template = await _templateRepository.GetAsync(x => x.Id == serviceMail.TemplateId);
            content = new StringBuilder(template.DefaultTemplate);
            foreach (var item in serviceMail.ExtraProperties)
            {
                content = content.Replace(item.Key, item.Value.ToString());
            }

            return content.ToString();
        }
    }
}
