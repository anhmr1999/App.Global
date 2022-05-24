using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace App.Global.Entitis.Emails
{
    public class EmailConfig : FullAuditedAggregateRoot<Guid>
    {
        public string Code { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
