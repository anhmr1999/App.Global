using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace App.Global.Entitis.Organizations
{
    public class Commune : FullAuditedAggregateRoot<Guid>
    {
        public Commune()
        {

        }

        public Commune(string code, string name, string districtCode)
        {
            Code = code;
            Name = name;
            DistrictCode = districtCode;
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string DistrictCode { get; set; }
    }
}
