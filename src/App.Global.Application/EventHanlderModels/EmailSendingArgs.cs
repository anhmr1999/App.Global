using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.BackgroundJobs;

namespace App.Global.EventHanlderModels
{
    public class EmailSendingArgs
    {
        public Guid EmailId { get; set; }
    }
}
