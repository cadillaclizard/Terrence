using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace Terrence.Services
{
    [TerrenceService("google")]
    class GoogleSearchByImage : ITerrenceService
    {
        private const string requestUrl = "http://www.google.com/searchbyimage/upload";
        private const string userAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:32.0) Gecko/20100101 Firefox/32.0";
        private readonly Encoding encoding = Encoding.UTF8;
        private readonly string contentType;
        private readonly string boundary;

        public GoogleSearchByImage()
        {
            var guid = Guid.NewGuid().ToString();
            boundary = new string('-', 40 - guid.Length) + guid;
            contentType = "multipart/form-data; boundary=" + boundary.Substring(2);
        }

        public void Run(string[] args)
        {
            var imageFilePath = args[0];
            var imageBytes = File.ReadAllBytes(imageFilePath);

            var request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.Method = "POST";
            request.ContentType = contentType;
            request.UserAgent = userAgent;

            using (var requestData = new MemoryStream())
            {
                // File
                var content = boundary + "\r\nContent-Disposition: form-data; name=\"encoded_image\"; filename=\"texture.png\"\r\nContent-Type: image/png\r\n\r\n";
                requestData.Write(encoding.GetBytes(content), 0, encoding.GetByteCount(content));
                requestData.Write(imageBytes, 0, imageBytes.Length);
                // Footer
                content = "\r\n" + boundary + "--\r\n";
                requestData.Write(encoding.GetBytes(content), 0, encoding.GetByteCount(content));

                var buffer = new byte[requestData.Length];
                requestData.Position = 0;
                requestData.Read(buffer, 0, buffer.Length);
                requestData.Close();

                request.ContentLength = buffer.Length;
                using (var requestStream = request.GetRequestStream())
                    requestStream.Write(buffer, 0, buffer.Length);
            }

            var response = request.GetResponse();
            Process.Start(response.ResponseUri.ToString());
        }
    }
}
