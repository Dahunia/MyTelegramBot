namespace MyTelegramBot.Dtos.Telegram
{
    public class AnswerCallbackQueryDto
    {
        public AnswerCallbackQueryDto()
        {
            this.show_alert = true;
            this.cache_time = 60;
        }
        public string callback_query_id { get; set; }
        public string text { get; set; }
        public bool show_alert { get; set; }
        public int cache_time { get; set; }
    }
}