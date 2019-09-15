using System.Collections.Generic;

namespace MyTelegramBot.Models.Telegram
{
    public class User
    {
        public int Id { get; set; }
        public bool IsBot { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string UserName { get; set; }
        public string LanguageCode { get; set; }
        public List<Message> Messages { get; set; }
    }
}