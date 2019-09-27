namespace MyTelegramBot.Models.Telegram
{
    public class Entity
    {
        public long Id { get; set; }
        public int Offset { get; set; }
        public int Length { get; set; }
        public string Type { get; set; }
    }
}