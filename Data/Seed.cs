using System.Collections.Generic;
using System.Threading.Tasks;
using MyTelegramBot.Models.Telegram;
using Newtonsoft.Json;

namespace MyTelegramBot.Data
{
    public class Seed
    {
        private readonly DataContext _context;
        public Seed(DataContext context) => _context = context;
        public void SeedCategories() 
        {
            var categoriesData = System.IO.File.ReadAllText("Data/json/CategoriesSeedData.json");
            var categories = JsonConvert.DeserializeObject<List<Category>>(categoriesData);
            _context.AddRange(categories);
             _context.SaveChanges();
        }
    }
}