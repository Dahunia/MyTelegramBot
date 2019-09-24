using System.Collections.Generic;

namespace MyTelegramBot.Models.Telegram
{
    public class User
    {
        public long Id { get; set; }
        //public long FromId { get; set; }
        public bool IsBot { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string UserName { get; set; }
        public string LanguageCode { get; set; } 
        public System.DateTime Created { get; set; }
        public System.DateTime LastActive { get; set; }
        public ICollection<Message> MessagesSent { get; set; } // Отправил
        public ICollection<CallbackQuery> CallbackQueriesSent { get; set; }
        public ICollection<Result> ResultsSent { get; set ;}
        //public List<Chat> SentToChats { get; set; }
        //public List<Message> MessageReceived { get; set; } // Получил
    }
}