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
using Volo.Abp.Settings;

namespace App.Global.Commons.Helpers
{
    public class EmailHelper : ITransientDependency
    {
        private readonly IRepository<Service_SendMail> _emailRepository;
        private readonly IRepository<EmailTemplate> _templateRepository;
        private readonly ISettingProvider _settingProvider;

        public EmailHelper(
            IRepository<Service_SendMail> emailRepository,
            IRepository<EmailTemplate> templateRepository,
            ISettingProvider settingProvider)
        {
            _emailRepository = emailRepository;
            _templateRepository = templateRepository;
            _settingProvider = settingProvider;
        }

        public async Task SendMailAsync(Guid EmailId)
        {
            var serviceMail = await _emailRepository.GetAsync(x => x.Id == EmailId);
            var settingValues = await _settingProvider.GetAllAsync();
            var Settings = new EmailSettingsDto()
            {
                DefaultFromDisplayName = settingValues.FirstOrDefault(x => x.Name == "Abp.Mailing.DefaultFromAddress")?.Value,
                SmtpHost = settingValues.FirstOrDefault(x => x.Name == "Abp.Mailing.Smtp.Host")?.Value,
                SmtpPort = Int32.Parse(settingValues.FirstOrDefault(x => x.Name == "Abp.Mailing.Smtp.Port").Value),
                SmtpUserName = settingValues.FirstOrDefault(x => x.Name == "Abp.Mailing.Smtp.UserName")?.Value,
                SmtpPassword = settingValues.FirstOrDefault(x => x.Name == "Abp.Mailing.Smtp.Password")?.Value,
                SmtpEnableSsl = settingValues.FirstOrDefault(x => x.Name == "Abp.Mailing.Smtp.EnableSsl").Value == "True"
            };
            if (serviceMail != null)
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
                    catch (Exception ex)
                    {
                        serviceMail.Status = (int)EmailStatusEnum.Fail;
                        await _emailRepository.UpdateAsync(serviceMail);
                    }
                }
            }
        }

        public async Task<string> GetEmailContent(Guid emailId)
        {
            var email = await _emailRepository.FirstOrDefaultAsync(x => x.Id == emailId);
            if (email == null)
                return string.Empty;
            return await GetEmailContent(email);
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
