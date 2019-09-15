namespace MyTelegramBot.Models.Telegram
{
    public class Product
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string CodeLanguage { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public double? Price { get; set; }
    }
}