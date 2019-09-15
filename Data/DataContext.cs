using Microsoft.EntityFrameworkCore;
using MyTelegramBot.Models.Telegram;

namespace MyTelegramBot.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DataContext(DbContextOptions<DataContext> options)
            :base(options)
        {   
            Database.EnsureCreated();
        }
       
    }
}