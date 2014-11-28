using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace Terrence
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length == 0)
                return;

            try
            {
                var filePath = String.Join(" ", args);
                var fileData = File.ReadAllBytes(filePath);
                var encoding = Encoding.UTF8;
                var guid = Guid.NewGuid().ToString();
                var boundary = new string('-', 40 - guid.Length) + guid;
                var contentType = "multipart/form-data; boundary=" + boundary.Substring(2);
                const string userAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:32.0) Gecko/20100101 Firefox/32.0";
                const string requestUrl = "http://www.google.com/searchbyimage/upload";

                Stream data = new MemoryStream();

                var str = boundary + "\r\n" +
                          "Content-Disposition: form-data; name=\"image_url\"\r\n" +
                          "\r\n\r\n" +
                          boundary + "\r\n" +
                          "Content-Disposition: form-data; name=\"encoded_image\"; filename=\"texture.png\"" + "\r\n" +
                          "Content-Type: image/png" + "\r\n\r\n";
                data.Write(encoding.GetBytes(str), 0, encoding.GetByteCount(str));
                data.Write(fileData, 0, fileData.Length);
                str = "\r\n" +
                      boundary + "\r\n" +
                      "Content-Disposition: form-data; name=\"image_content\"" + "\r\n" +
                      "\r\n\r\n" +
                      boundary + "\r\n" +
                      "Content-Disposition: form-data; name=\"filename\"" + "\r\n" +
                      "\r\n\r\n" +
                      boundary + "--\r\n";
                data.Write(encoding.GetBytes(str), 0, encoding.GetByteCount(str));

                var bytes = new byte[data.Length];
                data.Position = 0;
                data.Read(bytes, 0, bytes.Length);
                data.Close();

                var request = (HttpWebRequest) WebRequest.Create(requestUrl);
                request.Method = "POST";
                request.ContentType = contentType;
                request.UserAgent = userAgent;
                request.ContentLength = bytes.Length;

                using (var requestStream = request.GetRequestStream())
                    requestStream.Write(bytes, 0, bytes.Length);

                var response = (HttpWebResponse) request.GetResponse();

                Process.Start(response.ResponseUri.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadKey();
            }
        }
    }
}
