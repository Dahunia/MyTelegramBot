using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Data.Interface;
using MyTelegramBot.Dtos.Telegram;

namespace MyTelegramBot.Checkers.Messages
{
    public class AuthChecker : AbstractMessageChecker
    {
        private readonly IAuthRepository _repo;
        public AuthChecker(
            IAuthRepository repo,
            IServiceProvider provider)//ILoggerFactory loggerFactory, IMyLogger filelogger, ITelegramApiRequest telegramRequest) 
            : base(provider)
        => _repo = repo;

        public override Task<object> Checker(MessageDto incomingMessageDto)
        {
        
            return base.Checker(incomingMessageDto);
        }
    }
}