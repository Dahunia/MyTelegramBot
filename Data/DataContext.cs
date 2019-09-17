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
        => Database.EnsureCreated();
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(m => m.From)
                .WithMany(u => u.MessagesSent)
                .HasForeignKey(m => m.FromId)
                .OnDelete(DeleteBehavior.Restrict);
 
            builder.Entity<Message>()
                .HasOne(m => m.Chat)
                .WithMany(c => c.MessagesReceived)
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Restrict); 
        }
       
    }
}