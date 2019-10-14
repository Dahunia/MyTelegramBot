using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Interface;
using Microsoft.Extensions.DependencyInjection;
using MyTelegramBot.Helpers;

namespace MyTelegramBot.Checkers.Messages
{
    public abstract class AbstractMessageChecker : IMessageChecker
    {
        private IMessageChecker _nextChecker;
        private IMyLogger<AbstractMessageChecker> _logger;
        private IBackwardRepository _backwardRepository;
        private ITelegramRequest _telegramRequest;

        protected AbstractMessageChecker(
            IMyLogger<AbstractMessageChecker> logger, 
            IBackwardRepository backwardRepository, 
            ITelegramRequest telegramRequest)
        {
            _telegramRequest = telegramRequest;
            _logger = logger;
            _backwardRepository = backwardRepository;
        }

        public IMessageChecker SetNext(IMessageChecker checker)
        {
            _nextChecker = checker;

            return checker;
        }
        public virtual async Task<string> Checker(MessageDto incomingMessageDto)
        {
            if (this._nextChecker != null)
            {
                return await this._nextChecker.Checker(incomingMessageDto);
            }
            else {
                return "";
            }
        }

         public async Task<string> SendView<T>(
            MessageForSendDto<T> messageForSendDto, 
            MessageDto incomingMessageDto) 
            where T: class
        {
            long userId = incomingMessageDto.From.Id;
            if (messageForSendDto.isEqual(incomingMessageDto)) 
            {            
                return "";
            }
            var response = await _telegramRequest.SendMessage(messageForSendDto) ?? "";

            //await _backwardRepository.SetBackwardView(userId, messageForSendDto);
            await _logger.LogSentToUser(messageForSendDto);

            return response;
        }
    }
}