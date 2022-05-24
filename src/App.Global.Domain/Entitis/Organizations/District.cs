using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace App.Global.Entitis.Organizations
{
    public class District : FullAuditedAggregateRoot<Guid>
    {
        public District(string code, string name, string provinceCode)
        {
            Code = code;
            Name = name;
            ProvinceCode = provinceCode;
        }
        public District()
        {

        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string ProvinceCode { get; set; }
    }
}
