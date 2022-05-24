using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Global.Commons.Helpers
{
    public enum EmailStatusEnum
    {
        Created = 0,
        Processing = 1,
        Done = 2,
        Fail = 3,
        ReSended = 4
    }
}
