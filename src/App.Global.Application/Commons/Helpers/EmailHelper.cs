using App.Global.Entitis.Emails;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace App.Global.Commons.Helpers
{
    public class EmailHelper : ITransientDependency
    {
        private readonly IRepository<EmailConfig> _emailConfigRepository;
        private readonly IRepository<Service_SendMail> _emailRepository;
        private readonly IUnitOfWorkManager _uow;

        public EmailHelper(
            IRepository<EmailConfig> emailConfigRepository,
            IRepository<Service_SendMail> emailRepository,
            IUnitOfWorkManager uow)
        {
            _emailConfigRepository = emailConfigRepository;
            _emailRepository = emailRepository;
            _uow = uow;
        }

        public async Task SendEmail(Service_SendMail email)
        {
            
        }
        public async Task SendEmail(EmailInput emailInfor)
        {
            var email = await _emailRepository.FirstOrDefaultAsync(x => x.Id == emailInfor.EmailId);
            if (!string.IsNullOrWhiteSpace(emailInfor.Content) && !string.IsNullOrWhiteSpace(emailInfor.Mail_To) && email != null)
            {
                try
                {
                    emailInfor = await SetConfigData(emailInfor);
                    using (var client = new SmtpClient())
                    {
                        var credential = new NetworkCredential
                        {
                            UserName = emailInfor.Account,
                            Password = emailInfor.Password
                        };
                        client.Host = emailInfor.Smtp;
                        client.UseDefaultCredentials = false;
                        client.Credentials = credential;
                        client.EnableSsl = emailInfor.SSL;
                        client.Port = int.Parse(emailInfor.Port);

                        using (var emailMessage = new MailMessage())
                        {
                            //emailMessage.To.Add(new MailAddress(sm.NguoiNhan_Email));

                            foreach (var address in emailInfor.Mail_To.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                emailMessage.To.Add(address);
                            }

                            emailMessage.From = new MailAddress(emailInfor.Mail_From, emailInfor.Mail_Name);
                            emailMessage.Subject = emailInfor.Title;
                            if (emailInfor.HtmlView)
                            {
                                AlternateView alternateView = ContentToAlternateView(emailInfor.Content);
                                emailMessage.AlternateViews.Add(alternateView);
                                //emailMessage.Body = sm.Email_NoiDung;
                                emailMessage.IsBodyHtml = true;
                            }
                            else
                            {
                                emailMessage.Body = emailInfor.Content;
                            }

                            client.Send(emailMessage);

                        }
                    }
                    using (var unitOfWork = _uow.Begin())
                    {
                        email.Status = (int)EmailStatusEnum.Done;
                        await _emailRepository.UpdateAsync(email);
                        await unitOfWork.CompleteAsync();
                    }
                }
                catch (Exception ex)
                {
                    using (var unitOfWork = _uow.Begin())
                    {
                        email.Status = (int)EmailStatusEnum.Fail;
                        await _emailRepository.UpdateAsync(email);
                        await unitOfWork.CompleteAsync();
                    }
                }
            }
        }

        private async Task<EmailInput> SetConfigData(EmailInput emailInfor)
        {
            var lstConfig = await _emailConfigRepository.GetQueryableAsync();
            emailInfor.Mail_From = lstConfig.FirstOrDefault(x => x.Code == "MAIL_FROM").Value;
            emailInfor.Account = lstConfig.FirstOrDefault(x => x.Code == "MAIL_ACCOUNT").Value;
            emailInfor.Password = lstConfig.FirstOrDefault(x => x.Code == "MAIL_PASSWORD").Value;
            emailInfor.SSL = lstConfig.FirstOrDefault(x => x.Code == "MAIL_SSL").Value.ToLower() == "true";
            emailInfor.Mail_Name = lstConfig.FirstOrDefault(x => x.Code == "MAIL_NAME").Value;
            emailInfor.Smtp = lstConfig.FirstOrDefault(x => x.Code == "MAIL_SMTP").Value;
            emailInfor.Port = lstConfig.FirstOrDefault(x => x.Code == "MAIL_PORT").Value;
            return emailInfor;
        }

        private AlternateView ContentToAlternateView(string content)
        {
            content = Normalizecss(content);
            var imgCount = 0;
            List<LinkedResource> resourceCollection = new List<LinkedResource>();
            foreach (Match m in Regex.Matches(content, "<img(?<value>.*?)>"))
            {
                imgCount++;
                var imgContent = m.Groups["value"].Value;
                string type = Regex.Match(imgContent, ":(?<type>.*?);base64,").Groups["type"].Value;
                string base64 = Regex.Match(imgContent, "base64,(?<base64>.*?)\"").Groups["base64"].Value;
                if (String.IsNullOrEmpty(type) || String.IsNullOrEmpty(base64))
                {
                    //ignore replacement when match normal <img> tag
                    continue;
                }
                var replacement = " src=\"cid:" + imgCount + "\"";
                content = content.Replace(imgContent, replacement);

                byte[] imageBytes = Convert.FromBase64String(base64);

                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                var tempResource = new LinkedResource(ms, new ContentType(type))
                {
                    ContentId = imgCount.ToString()
                };
                resourceCollection.Add(tempResource);
            }
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);
            foreach (var item in resourceCollection)
            {
                alternateView.LinkedResources.Add(item);
            }
            return alternateView;
        }
        private string Normalizecss(string content)
        {
            return content
                .Replace("class=\"ql-align-center\"", "style=\"text-align: center;\"")
                .Replace("class=\"ql-align-right\"", "style=\"text-align: right;\"")
                .Replace("class=\"ql-font-monospace\"", "style=\"font-family: Monaco,Courier New,monospace;\"")
                .Replace("class=\"ql-font-serif\"", "style=\"font-family: Georgia,Times New Roman,serif;\"")
                ;
        }
    }
}
