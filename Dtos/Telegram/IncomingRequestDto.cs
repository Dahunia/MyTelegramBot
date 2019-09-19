namespace MyTelegramBot.Dtos.Telegram
{
    public class IncomingRequestDto
    {
        public long update_id { get; set; }
        public MessageDto message { get; set; }
        public CallbackQueryDto callback_query { get; set;}
        public ResponseDto response { get; set ;}
    }
}