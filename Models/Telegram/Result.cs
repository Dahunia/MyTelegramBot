namespace MyTelegramBot.Models.Telegram
{
    public class Result
    {
        public long Id { get; set; }
        public long MessageId { get; set; }
        public long FromId { get; set; }
        public User From { get; set; }
        public long ChatId { get; set; }
        public Chat Chat { get; set; }
        public long Date { get; set; }
        public string Text { get; set; }
    }
}