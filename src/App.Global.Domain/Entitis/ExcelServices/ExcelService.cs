using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace App.Global.Entitis.ExcelServices
{
    public class ExcelService : FullAuditedAggregateRoot<Guid>
    {
        public ExcelStatusEnum Status { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Message { get; set; }
        public bool ExportFile { get; set; }
    }
}
