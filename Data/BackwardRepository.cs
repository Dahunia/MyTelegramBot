using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Interface;
using MyTelegramBot.Models.Telegram;

namespace MyTelegramBot.Data
{
    public class BackwardRepository: IBackwardRepository
    {
        private readonly IMemoryCache _cache;

        public BackwardRepository(IMemoryCache cache) => _cache = cache;
       
        public async Task SetBackwardCommand(long userId, string command) 
        {
            command = command.ToLower();
            await Task.Run(() => {
                if(command.Equals(@"/start"))
                {
                    ClearCommands(userId);
                }
                if(!command.Equals(@"/backward"))
                {
                    PushCommand(userId, command);
                }
            });
        }
        public async Task SetBackwardView(
            long userId, MessageTextForEditDto messageTextForEditDto) => 
            await Task.Run(() => 
                _cache.Set(userId, messageTextForEditDto, GetCacheEntryOptions(5)));

        public async Task SetBackwardView<TButton>(
            long userId, MessageForSendDto<TButton> messageForSendDto) where TButton: class => 
            await Task.Run(() => 
                _cache.Set(userId, messageForSendDto, GetCacheEntryOptions(5)));
        
        public async Task<string> GetBackwardCommand(long userId)
        {
            return await Task.Run(() => 
            {
                return PopCommand(userId) ?? @"/start";
            });       
        }

        public async Task<string> GetBackwardCommands(long userId)
        {
            return await Task.Run(() => 
            {
                var commands = 
                //copy myStack.ToArray()
                //string.join(array);
                return PopCommand(userId) ?? @"/start";
            });       
        }
        public async Task<object> GetView (long userId)
        {
            return await PopView(userId);
            /*if (backwardToReturn is MessageForSendDto<InlineKeyboardMarkup>) {
                return backwardToReturn as MessageForSendDto<InlineKeyboardMarkup>;
            }
            if (backwardToReturn is MessageTextForEditDto)
                return backwardToReturn as MessageTextForEditDto;
            return null; */
        }
        private MemoryCacheEntryOptions GetCacheEntryOptions(int time) =>
            new MemoryCacheEntryOptions {
                AbsoluteExpirationRelativeToNow = System.TimeSpan.FromMinutes(5)
            };

        private void PushCommand(long userId, string command)
        {
            Stack<string> commands = null;
            if (!_cache.TryGetValue(userId, out commands))
            {
                commands = new Stack<string>();
            }
            commands.Push(command);
            _cache.Set(userId, commands);
        }

        private string PopCommand(long userId)
        {
            Stack<string> commands = null;
            if (_cache.TryGetValue(userId, out commands))
            {
                var backCommand = commands.Pop();
                _cache.Set(userId, commands);
                return backCommand;
            }
            else {
                return null;
            }
        }
        private void ClearCommands(long userId) =>
            _cache.Set(userId, new Stack<string>());

        private async Task PushView<T>(long userId, T view)
        {
            await Task.Run(() => {
                Stack views = null;
                if(_cache.TryGetValue(userId, out views))
                {
                    views = new Stack();
                }
                views.Push(view);
                _cache.Set(userId, views);
            });
        }

        private async Task<object> PopView(long userId)=>
            await Task.Run(() => {
                Stack views = null;
                if (_cache.TryGetValue(userId, out views))
                {
                    var view = views.Pop();
                    _cache.Set(userId, views);
                    return view;
                }
                else {
                    return null;
                }
            });
        public async Task<MessageForSendDto<TButton>> GetBackwardMessageForSendView<TButton>(long userId)
        where TButton: class
        {
            return await Task.Run(() => 
            {
                MessageForSendDto<TButton> messageDto = null;
                if (_cache.TryGetValue(userId, out messageDto))
                {
                    return messageDto;
                }
                return null;
            });  
        }

        public async Task<MessageTextForEditDto> GetBackwardMessageTextForEditView(long userId)
        {
            return await Task.Run(() => 
            {
                MessageTextForEditDto messageDto = null;
                if (_cache.TryGetValue(userId, out messageDto))
                {
                    return messageDto;
                }
                return null;
            });  
        }
    }
}

/* public async Task<MessageTextForEditDto> GetBackwardMessageTextForEditView(long userId)
{
    return await Task.Run(() => 
    {
        MessageTextForEditDto messageDto = null;
        if (_cache.TryGetValue(userId, out messageDto))
        {
            return messageDto;
        }
        return null;
    });  
}
public async Task<MessageForSendDto<TButton>> GetBackwardMessageForSendView<TButton>(long userId)
    where TButton: class
{
    return await Task.Run(() => 
    {
        MessageForSendDto<TButton> messageDto = null;
        if (_cache.TryGetValue(userId, out messageDto))
        {
            return messageDto;
        }
        return null;
    });  
} */