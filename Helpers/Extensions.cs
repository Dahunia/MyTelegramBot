using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace MyTelegramBot.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
        public static string ReadRequestBody(this HttpRequest request) 
        {
            string body = "";
            
            if (request.ContentLength == null ||
                !(request.ContentLength > 0) ||
                !request.Body.CanSeek) 
            {
                return body;
            }

            //request.Body.Seek(0, System.IO.SeekOrigin.Begin);
            using(var reader = new System.IO.StreamReader(request.Body))//, Encoding.Default, true, 1024, true))
            {
                body = reader.ReadToEnd();
            }
            return body;
            //return body.Replace("\t", "").Replace("\n", "");
            //return Regex.Replace(body, "[^a-zA-Z0-9_.]", "", RegexOptions.Compiled);
        }

        public static System.DateTime CalculateDateTime(this System.UInt64 dateUtc)
        {
            var dateNormal = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            dateNormal = dateNormal.AddSeconds(dateUtc).ToLocalTime();
            return dateNormal;
        }
    }
}