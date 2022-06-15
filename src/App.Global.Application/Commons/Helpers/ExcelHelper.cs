using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.Collections.Generic;
using System.IO;
using Volo.Abp.DependencyInjection;

namespace App.Global.Commons.Helpers;

public class ExcelHelper : ITransientDependency
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ExcelHelper(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public void Export<T>(string fileName, string[] headers, List<T> records) where T : class
    {
        var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "Excels", "Exports", fileName);
        using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Data");
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 2].Value = headers[i];
                worksheet.Cells[1, i + 2].Style.Font.Bold = true;
                worksheet.Cells[1, i + 2].Style.Font.Size = 20;
            }
            worksheet.Cells[2, 2].LoadFromCollection(records);

            var range = worksheet.Cells[1, 1, records.Count + 1, headers.Length + 1];
            worksheet.Tables.Add(range, "DataExport").TableStyle = TableStyles.Medium6;

            //Load index
            worksheet.Cells[1, 1].Value = "";
            worksheet.Cells[1, 1].Style.Font.Bold = true;
            worksheet.Cells[1, 1].Style.Font.Size = 20;
            for (int i = 1; i <= records.Count; i++)
                worksheet.Cells[i + 1, 1].Value = i;
            worksheet.Cells.AutoFitColumns();

            package.Save();
        }
    }
}