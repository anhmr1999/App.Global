using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace App.Global.Entitis.Organizations
{
    public class Province : FullAuditedAggregateRoot<Guid>
    {
        public Province(string code, string name)
        {
            Code = code;
            Name = name;
        }
        public Province()
        {
        }

        public string Code { get; set; }
        public string Name { get; set; }
    }
}
