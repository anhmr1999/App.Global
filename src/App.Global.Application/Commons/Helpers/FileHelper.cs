using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace App.Global.Commons.Helpers
{
    public class FileHelper : ITransientDependency
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";

        public FileHelper(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<string> SaveFileAsync(IFormFile file)
        {
            var fileName = Guid.NewGuid() + file.FileName;
            var fileLocation = Path.Combine(_webHostEnvironment.WebRootPath, USER_CONTENT_FOLDER_NAME);
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, USER_CONTENT_FOLDER_NAME, fileName);
            if (!Directory.Exists(fileLocation))
            {
                Directory.CreateDirectory(fileLocation);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var linkfile = $"/{USER_CONTENT_FOLDER_NAME}/{fileName}";
            return linkfile;
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var filePath = $@"{_webHostEnvironment.WebRootPath}{fileName}";
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }
    }
}
