using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Models.Telegram;
using Newtonsoft.Json;

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

            request.Body.Seek(0, System.IO.SeekOrigin.Begin);
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

        public static JsonSerializerSettings GetJsonSerializerSettings<T>(ref bool isError)
            where T: class
        {
            return new JsonSerializerSettings() {
                Error = (s, e) => {
                    var purpose = e.CurrentObject as T;
                }
            };
        }

        public static bool isEqual(
            this MessageTextForEditDto messageTextForEditDto,
            MessageDto incomingMessageDto)
        {
            return (messageTextForEditDto.ChatId == incomingMessageDto.Chat.Id)
                && (messageTextForEditDto.MessageId == incomingMessageDto.Id)
                && (messageTextForEditDto.Text.ToLower().Equals(incomingMessageDto.Text))
                && (messageTextForEditDto.ReplyMarkup == incomingMessageDto.ReplyMarkup);
/*                 (JsonConvert.SerializeObject(messageTextForEditDto.ReplyMarkup).Equals(
                    JsonConvert.SerializeObject(incomingMessageDto.)
                ) */
        }

        public static bool isEqual<T>(
            this MessageForSendDto<T> messageForSendDto,
            MessageDto incomingMessageDto)
        where T: class
        {
           return (messageForSendDto.Text == incomingMessageDto.Text)
               && (messageForSendDto.ReplyMarkup == incomingMessageDto.ReplyMarkup)
               && (messageForSendDto.ChatId == incomingMessageDto.Chat.Id);
        }
        
    }
}