using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace App.Global.ExcelModels
{
    public class EmailExport
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ReceiverEmail { get; set; }
        public int NumberOfTimeSend { get; set; }
        public string Status { get; set; }
    }
}
