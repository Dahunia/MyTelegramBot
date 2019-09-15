namespace MyTelegramBot.Dtos.Telegram
{
    public class ResultDto
    {
        public long update_id { get; set; }
        public MessageDto message { get; set; }
    }
}