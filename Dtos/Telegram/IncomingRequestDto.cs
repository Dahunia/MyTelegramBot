namespace MyTelegramBot.Dtos.Telegram
{
    public class IncomingRequestDto
    {
        public long update_id { get; set; }
        public Message message { get; set; }
        public Callback_Query callback_query { get; set;}
    }
}