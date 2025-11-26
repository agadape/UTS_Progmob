using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTalk.Shared.Services
{
    public class CapturedPhoto
    {
        public string FileName { get; set; } = "";
        public string ContentType { get; set; } = "image/jpeg";
        public Stream Stream { get; set; } = Stream.Null;
    }
    public interface ICameraService
    {
        Task<CapturedPhoto?> CapturePhotoAsync();
    }
}
