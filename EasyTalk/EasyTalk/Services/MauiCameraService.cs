using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyTalk.Shared.Services;

namespace EasyTalk.Services
{
    internal class MauiCameraService : ICameraService
    {
        public async Task<CapturedPhoto?> CapturePhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo == null)
                    return null;

                var stream = await photo.OpenReadAsync();

                return new CapturedPhoto
                {
                    FileName = photo.FileName,
                    ContentType = photo.ContentType ?? "image/jpeg",
                    Stream = stream
                };
            }
            catch
            {
                // Bisa kamu log kalau mau
                return null;
            }
        }

    }
}
