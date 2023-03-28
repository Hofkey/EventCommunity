using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventCommunity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {

        public FileController() { }

        [HttpGet]
        public IActionResult DownloadFiles([FromQuery] IEnumerable<Guid> fileIds)
        {

        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles([FromForm] IEnumerable<IFormFile> files)
        {
            if (files == null || !files.Any())
                return BadRequest("No files were uploaded.");

            var fileInfos = new List<FileInfo>();

            foreach (var file in files)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine("path/to/files", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                fileInfos.Add(new FileInfo(filePath));
            }

            return Ok(fileInfos);
        }
    }
}
