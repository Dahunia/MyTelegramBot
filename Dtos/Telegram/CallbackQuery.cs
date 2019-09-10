using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;

namespace MyTelegramBot.Dtos.Telegram
{
    public class CallbackQuery// : IOutboundParameterTransformer
    {
        public string id { get; set; }
        public From from { get; set; }
        public Message message { get; set; }
        public string chat_instance { get; set; }
        public string data { get; set; }
     /*    public string TransformOutbound(object value)
        {
            var result = value == null ? 
                null :
                Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2").ToLower();
            return result;
        } */
    }
}