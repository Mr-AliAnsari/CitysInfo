using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CitysInfo.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider
                ?? throw new System.ArgumentNullException(
                    nameof(fileExtensionContentTypeProvider));
        }

        [HttpGet("{fileId}")]
        [ApiVersion(0.1, Deprecated = true)]
        public ActionResult GetFile(string fileId)
        {
            string pathToFile = "zahedan.pdf";
            if (!System.IO.File.Exists(pathToFile))
            {
                return NotFound();
            }

            var bytes = System.IO.File.ReadAllBytes(pathToFile);
            if (!_fileExtensionContentTypeProvider.TryGetContentType
                (fileId, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            return File(bytes, contentType, Path.GetFileName(pathToFile));
        }

        [HttpPost]
        public async Task<ActionResult> CreateFile(IFormFile formFile)
        {
            // بررسی شرایط معتبر بودن فایل
            if (formFile == null || formFile.Length == 0 ||
                formFile.Length > 20971520 ||
                formFile.ContentType != "application/pdf")
            {
                return BadRequest("No file or an invalid one has been inputted.");
            }

            // ایجاد مسیر ذخیره‌سازی فایل
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

            // اطمینان از وجود دایرکتوری
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var filePath = Path.Combine(uploadPath, $"uploaded_formFile_{Guid.NewGuid()}.pdf");

            try
            {
                //filePath مسیر فایلی است که می‌خواهیم آن را ایجاد یا بازنویسی کنیم.
                //FileMode.Create به این معناست که اگر فایلی با نام مشابه وجود داشته باشد،
                //آن را پاک کرده و یک فایل جدید ایجاد کند.
                //اگر فایل وجود نداشته باشد، یک فایل جدید ایجاد خواهد کرد.

                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    //کپی کردن محتویات فرم فایل در فایل استریم
                    await formFile.CopyToAsync(filestream);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return Ok("Your file has been uploaded successfully.");
        }

    }
}
