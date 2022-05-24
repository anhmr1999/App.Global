using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace App.Global.Commons.Helpers.Excels
{
    public interface IEmailCommonHandler<T> : ITransientDependency
    {
        string ValidateHeader(ExcelWorksheet worksheet);
        string ValidateAndGetData(ExcelWorksheet worksheet, IEnumerable<T> collection);
    }
}
