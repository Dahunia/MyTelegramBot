using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;

namespace MyTelegramBot.Dtos.Telegram
{
    public class Callback_Query
    {
        public string id { get; set; }
        public From from { get; set; }
        public Message message { get; set; }
        public string chat_instance { get; set; }
        public string data { get; set; }
    }
}