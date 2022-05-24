using App.Global.Entitis.Emails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Global.Commons.Helpers
{
    public class EmailInput
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public string Smtp { get; set; }
        public string Port { get; set; }
        public bool SSL { get; set; }
        public string Mail_From { get; set; }
        public string Mail_Name { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public string Mail_To { get; set; }
        public bool HtmlView { get; set; }
        public Guid EmailId { get; set; }
    }
}
