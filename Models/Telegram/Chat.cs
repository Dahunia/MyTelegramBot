using System.Collections.Generic;

namespace MyTelegramBot.Models.Telegram
{
    public class Chat
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set;}
        //Полученные сообщения
        public ICollection<Message> MessagesReceived { get; set; }
        //public List<User> FromUsers { get; set;}
    }
}