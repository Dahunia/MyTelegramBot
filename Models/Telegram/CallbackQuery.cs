namespace MyTelegramBot.Models.Telegram
{
    public class CallbackQuery
    {
        public string Id { get; set; }
        //public long CallbackQueryId { get; set; }
        public long FromId { get; set; }
        public User From { get; set; }
        public long MessageId { get; set; }
        public Message Message { get; set; }
        public string ChatInstance { get; set; }
        public string Data { get; set; }
    }
}