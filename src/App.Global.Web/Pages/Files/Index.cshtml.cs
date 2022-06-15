using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace App.Global.Web.Pages.Files
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public UploadFileDto FileDto { get; set; }
        public void OnGet()
        {
        }

        public async Task OnPostAsync()
        { 
            using(var memoryStream = new MemoryStream())
            {
                await FileDto.File.CopyToAsync(memoryStream);
            }
        }
    }
    public class UploadFileDto
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile File { get; set; }

        [Required]
        [Display(Name = "Filename")]
        public string Name { get; set; }
    }
}
