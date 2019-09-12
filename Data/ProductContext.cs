using Microsoft.EntityFrameworkCore;
using MyTelegramBot.Models.Telegram;

namespace MyTelegramBot.Data
{
    public class ProductContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public ProductContext(DbContextOptions<ProductContext> options)
            :base(options)
        {   
            Database.EnsureCreated();
        }
       
    }
}