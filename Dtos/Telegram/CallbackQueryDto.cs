using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;

namespace MyTelegramBot.Dtos.Telegram
{
    public class CallbackQueryDto
    {
        public string id { get; set; }
        public FromDto from { get; set; }
        public MessageDto message { get; set; }
        public string chat_instance { get; set; }
        public string data { get; set; }
    }
}