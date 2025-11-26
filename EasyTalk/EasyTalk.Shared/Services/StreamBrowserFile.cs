using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTalk.Shared.Services
{
    internal class StreamBrowserFile : IBrowserFile
    {
        public StreamBrowserFile(string name, string contentType, Stream stream)
        {
            Name = name;
            ContentType = contentType;
            _stream = stream;
            LastModified = DateTimeOffset.Now;
            Size = stream.Length;
        }

        private readonly Stream _stream;

        public string Name { get; }
        public DateTimeOffset LastModified { get; }
        public long Size { get; }
        public string ContentType { get; }

        public Stream OpenReadStream(long maxAllowedSize = 5120000, CancellationToken cancellationToken = default)
        {
            return _stream;
        }

    
   }

}
