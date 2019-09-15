using System.ComponentModel;

namespace MyTelegramBot.Models.Telegram
{
    public class Message
    {
        public long Id { get; set; }
        [DisplayName("message_id")]
        public long MessageId { get; set; }
        public ulong Date { get; set; }
        public long ChatId { get; set; }
        public Chat Chat { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public string Test { get; set; }
    }
}