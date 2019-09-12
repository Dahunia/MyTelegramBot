namespace MyTelegramBot.Dtos.Telegram
{
    public class AnswerCallbackQueryDto
    {
        public string callback_query_id { get; set; }
        public string text { get; set; }
    }
}