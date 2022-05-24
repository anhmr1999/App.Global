using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace App.Global.DataTranferObjects.Emails
{
    public class EmailConfigRequestDto
    {
        public IEnumerable<EmailConfigDto> Configs { get; set; }
    }

    public class EmailConfigDto : EntityDto<Guid?>
    {
        public string Code { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
