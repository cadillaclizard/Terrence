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
        private const string RequestUrl = "http://www.google.com/searchbyimage/upload";
        private const string UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:32.0) Gecko/20100101 Firefox/32.0";
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly string _contentType;
        private readonly string _boundary;

        public GoogleSearchByImage()
        {
            var guid = Guid.NewGuid().ToString();
            _boundary = new string('-', 40 - guid.Length) + guid;
            _contentType = "multipart/form-data; boundary=" + _boundary.Substring(2);
        }

        public void Run(string[] args)
        {
            var imageFilePath = args[0];
            var imageBytes = File.ReadAllBytes(imageFilePath);

            var request = (HttpWebRequest)WebRequest.Create(RequestUrl);
            request.Method = "POST";
            request.ContentType = _contentType;
            request.UserAgent = UserAgent;

            using (var requestData = new MemoryStream())
            {
                // File
                var content = _boundary + "\r\nContent-Disposition: form-data; name=\"encoded_image\"; filename=\"texture.png\"\r\nContent-Type: image/png\r\n\r\n";
                requestData.Write(_encoding.GetBytes(content), 0, _encoding.GetByteCount(content));
                requestData.Write(imageBytes, 0, imageBytes.Length);
                // Footer
                content = "\r\n" + _boundary + "--\r\n";
                requestData.Write(_encoding.GetBytes(content), 0, _encoding.GetByteCount(content));

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
