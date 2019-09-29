namespace MyTelegramBot.Models.Telegram
{
    public class Update
    {
        public long Id { get; set; }
        //public long UpdateId { get; set; }
        public long MessageId { get; set; }
        public Message Message { get; set; }
        public long ResponseId { get; set; }
        public Response Response { get; set; }
        public string CallbackQueryId { get; set; }
        public CallbackQuery CallbackQuery { get; set; }
    }
}