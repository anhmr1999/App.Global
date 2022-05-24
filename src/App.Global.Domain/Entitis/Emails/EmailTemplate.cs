using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace App.Global.Entitis.Emails
{
    public class EmailTemplate : FullAuditedAggregateRoot<Guid>
    {
        public string TemplateName { get; set; }
        public string DefaultTitle { get; set; }
        public string DefaultTemplate { get; set; }
        public bool IsActive { get; set; }
        public bool AllowChange { get; set; }
    }
}
