using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;

namespace App.Global.DataTranferObjects.Emails
{
    public class Service_SendMailDto : EntityDto<Guid?>
    {
        public bool SystemEmail { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ReceiverEmail { get; set; }
        public int NumberOfTimeSend { get; set; }
        public int Status { get; set; }
        public ExtraPropertyDictionary ExtraProperties { get; set; }
        public Guid? TemplateId { get; set; }
        public string CreateUserName { get; set; }
        public EmailTemplateDto TemplateDto { get; set; }
    }
}
