namespace MyTelegramBot.Models.Telegram
{
    public class Update
    {
        public long Id { get; set; }
        //ublic long UpdateId { get; set; }
        public long MessageId { get; set; }
        public Message Message { get; set; }
        public long CalbackQueryId { get; set; }
        public CallbackQuery CallbackQuery { get; set; }

    }
}