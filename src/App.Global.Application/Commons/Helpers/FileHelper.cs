using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace App.Global.Commons.Helpers
{
    public class FileHelper : ITransientDependency
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileHelper(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<string> SaveFileAsync(IFormFile file, string filePath = "")
        {
            if (string.IsNullOrEmpty(filePath))
                filePath = GetFilePath(file.FileName);

            using (var stream = new FileStream(Path.Combine(_webHostEnvironment.WebRootPath, filePath), FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return filePath;
        }

        public async Task DeleteFileAsync(string filePath)
        {
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }

        public byte[] GetFileAsync(string filePath)
        {
            if (File.Exists(filePath))
                return File.ReadAllBytes(filePath);

            return null;
        }

        public static string GetFilePath(string fileName, string folderContent = "")
        {
            if (fileName.ToLower().Contains("content"))
                return fileName.Replace("content/", string.Empty);
            if (string.IsNullOrWhiteSpace(folderContent))
                folderContent = "default";
            return Path.Combine(folderContent, $"{Guid.NewGuid()}-{fileName}");
        }

        public static string CheckFileImage(IFormFile file)
        {
            var imagesExtensions = new List<string>()
                { ".jpg", ".png", ".svg", ".gif" };
            var extention = Path.GetExtension(file.FileName);
            if (!imagesExtensions.Any(x => x.Contains(extention)))
                return "Uploaded file is incorrect!";
            if (file.Length < GlobalConsts.imgMinSize || file.Length > GlobalConsts.imgMaxSize)
                return "Upload files must be between 5Kb and 3Mb!";
            return string.Empty;
        }
    }
}
