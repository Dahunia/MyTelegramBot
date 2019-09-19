namespace MyTelegramBot.Models.Telegram
{
    public class Response
    {
        public bool Ok { get; set; }
        public long ResultId { get; set; }
        public Result Result { get; set; }
        public System.DateTime DateSent { get; set; }
    }
}