using System.Text;
using Microsoft.AspNetCore.Http;

namespace MyTelegramBot.Helpers
{
    public static class Extensions
    {
        public static string ReadRequestBody(this HttpRequest request) 
        {
            string body = "";
            
            if (request.ContentLength == null ||
                !(request.ContentLength > 0) ||
                !request.Body.CanSeek) 
            {
                return body;
            }

            request.Body.Seek(0, System.IO.SeekOrigin.Begin);
            using(var reader = new System.IO.StreamReader(request.Body))//, Encoding.Default, true, 1024, true))
            {
                body = reader.ReadToEnd();
            }
            return body;
        }
    }
}