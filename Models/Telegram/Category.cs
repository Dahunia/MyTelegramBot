using System.Collections.Generic;

namespace MyTelegramBot.Models.Telegram
{
    public class Category
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string CodeLanguage { get; set; }
        public string Name { get; set; }
        public int Parent { get; set; }
        public List<Product> Products { get; set; }
    }
}