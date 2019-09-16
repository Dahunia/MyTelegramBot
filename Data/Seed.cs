using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            categories.ForEach(category => {

            });
           /*  _context.AddRange(
                categories.Where(newc => !_context.Categories.Include(c => c.Id).ToList()
             _context.SaveChanges(); */
        }
    }
}