using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;

namespace App.Global.DataTranferObjects.ExcelServices
{
    public class ExcelServiceDto : EntityDto, IHasCreationTime
    {
        public ExcelStatusEnum Status { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Message { get; set; }
        public bool ExportFile { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
