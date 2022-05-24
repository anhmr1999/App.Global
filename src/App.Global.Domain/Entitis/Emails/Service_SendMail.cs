using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace App.Global.Entitis.Emails
{
    public class Service_SendMail : FullAuditedAggregateRoot<Guid>
    {
        public bool SystemEmail { get; set; }
        public string Content { get; set; }
        public string ReceiverEmail { get; set; }
        public int NumberOfTimeSend { get; set; }
        public int Status { get; set; }
        public Guid? TemplateId { get; set; }
    }
}
