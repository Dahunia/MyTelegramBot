using System.Threading.Tasks;
using MyTelegramBot.Helpers;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Interface;

namespace MyTelegramBot.Checkers.Callback
{
    public abstract class AbstractCallbackChecker : ICallbackChecker
    {
        private ICallbackChecker _nextChecker;
        private ITelegramRequest _telegramRequest;
        private IMyLogger<AbstractCallbackChecker> _logger;
        private IBackwardRepository _backwardRepository;
        protected AbstractCallbackChecker( 
            IMyLogger<AbstractCallbackChecker> logger,
            IBackwardRepository backwardRepository,
            ITelegramRequest telegramRequest)
        {
            _logger = logger;
            _backwardRepository = backwardRepository; 
            _telegramRequest = telegramRequest;
        }

        public ICallbackChecker SetNext(ICallbackChecker checker)
        {
            _nextChecker = checker;

            return checker;
        }
        public virtual async Task<string> Checker(CallbackQueryDto incomingCallbackDto)
        {
            if (this._nextChecker != null)
            {
                return await this._nextChecker.Checker(incomingCallbackDto);
            }
            else {
                return "";
            }
        }
        public async Task<string> SendView(
            MessageTextForEditDto messageForSendDto, 
            CallbackQueryDto incomingCallbackDto) 
        {
            var userId = incomingCallbackDto.From.Id;
            if (messageForSendDto.isEqual(incomingCallbackDto.Message)) 
            {            
                return "";
            }
            var response = await _telegramRequest.SendChangeMessage(messageForSendDto) ?? "";

            //await _backwardRepository.SetBackwardView(userId, messageForSendDto);
            await _logger.LogSentToUser(messageForSendDto);

            return response;
        }
    }
}