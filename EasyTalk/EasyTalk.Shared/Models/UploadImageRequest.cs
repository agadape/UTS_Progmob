using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTalk.Shared.Models
{
    public class UploadImageRequest
    {
        public IFormFile File { get; set; }
    }
}
