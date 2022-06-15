using System;
using System.Collections.Generic;
using System.Text;

namespace App.Global
{
    public enum EmailStatusEnum
    {
        Created = 0,
        Processing = 1,
        Done = 2,
        Fail = 3,
        ReSended = 4
    }

    public enum ExcelStatusEnum
    {
        Created,
        Processing,
        Done,
        Fail
    }
}
