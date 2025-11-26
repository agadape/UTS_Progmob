using Microsoft.AspNetCore.Mvc;
using EasyTalk.Backend.Helpers;
using EasyTalk.Shared.Models;

namespace EasyTalk.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public UploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost("image")]
        [Consumes("multipart/form-data")]
        public IActionResult UploadImage([FromForm] UploadImageRequest request)
        {
            if (request.File == null)
                return BadRequest(new { error = "File is required" });

            var handler = new UploadHandler();
            var (success, message) = handler.UploadFile(request.File, _env);

            if (!success)
                return BadRequest(new { error = message });

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var fullUrl = $"{baseUrl}{message}";
            return Ok(new { url = fullUrl });

        }
    }
}
