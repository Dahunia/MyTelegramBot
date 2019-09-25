using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MyTelegramBot.Interface;
using MyTelegramBot.Models.Telegram;

namespace MyTelegramBot.Data
{
    public class AuthRepository : BaseRepository, IAuthRepository
    {
        //private readonly DataContext _context;
        private readonly IMemoryCache _cache;
        public AuthRepository(DataContext context, IMemoryCache memoryCache)
        // => _context = context;
        : base(context) { _cache = memoryCache; }
 
        public Task<User> Login(string user, string chat)
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> Register(User user)
        {
            await _context.AddAsync(user);
            if (await _context.SaveChangesAsync() > 0) 
            {
                _cache.Set(user.Id, user, GetCacheEntryOptions(5));
            }

            return user;
        }

         public async Task<Chat> Register(Chat chat)
        {
            await _context.AddAsync(chat);
            if (await _context.SaveChangesAsync() > 0)
            {
                _cache.Set(chat.Id, chat, GetCacheEntryOptions(5));
            };

            return chat;
        }

        public async Task<bool> UserExists(long userId)
        {
            return await GetUser(userId) != null;
        }
        public async Task<bool> ChatExists(long chatId)
        {
            return await GetChat(chatId) != null;
        }
        public async Task<User> GetUser(long userId)
        {
            User user;
            if (_cache.TryGetValue(userId, out user))
            {
                return user;
            }
            else if((user = await _context.Users.FindAsync(userId)) != null)
            {   
                _cache.Set(user.Id, user, GetCacheEntryOptions(5));
                return user;
            }
            return null;
        }
        public async Task<Chat> GetChat(long chatId)
        {
            Chat chat;
            if (_cache.TryGetValue(chatId, out chat))
            {
                return chat;
            }
            else if((chat = await _context.Chats.FindAsync(chatId)) != null)
            {   
                _cache.Set(chat.Id, chat, GetCacheEntryOptions(5));
                return chat;
            }
            return null;
        }
        
        private MemoryCacheEntryOptions GetCacheEntryOptions(int time) =>
            new MemoryCacheEntryOptions {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

/*         public async Task<bool> UpdateLastActive(User user)
        {
            user.LastActive = System.DateTime.Now;
            _context.Update(user);
        } */
    }
}