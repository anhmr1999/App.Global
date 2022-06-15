using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Global.EventHanlderModels
{
    public class ExcelImportArgs
    {
        public Guid ExcelId { get; set; }
    }

    public class ExcelExportArgs
    {
        public Guid ExcelId { get; set; }
        public string[] Headers { get; set; }
        public string Filter { get; set; }
        public int? Status { get; set; }
    }
}
