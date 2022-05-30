using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;

namespace App.Global.DataTranferObjects.Emails
{
    public class EmailTemplateDto : EntityDto<Guid?>
    {
        public string TemplateName { get; set; }
        public string DefaultTitle { get; set; }
        public string DefaultTemplate { get; set; }
        public ExtraPropertyDictionary ExtraProperties { get; set; }
        public bool IsActive { get; set; }
        public bool AllowChange { get; set; }
    }
}
