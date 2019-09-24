namespace MyTelegramBot.Models.Telegram
{
    public class Response
    {
        public long Id { get; set; }
        public bool Ok { get; set; }
        public long ResultId { get; set; }
        public Result Result { get; set; }
        public System.DateTime DateRecieved { get; set; }
    }
}