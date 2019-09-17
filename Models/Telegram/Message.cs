using System.ComponentModel;

namespace MyTelegramBot.Models.Telegram
{
    public class Message
    {
        public long Id { get; set; }
        //[DisplayName("message_id")]
        public long MessageId { get; set; }
        public ulong Date { get; set; }
        public long FromId { get; set; }
        public User From { get; set; }  // From whom - От кого
        public long ChatId { get; set; }
        public Chat Chat { get; set; } // Where - Куда
        public System.DateTime? DateRead { get; set; }
        public System.DateTime MessageSent { get; set; }
        public string Text { get; set; }
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }
    }
}