using System;
using System.Threading.Tasks;
using MyTelegramBot.Dtos.Telegram;

namespace MyTelegramBot.Checkers.Callback
{
    public class AbstractCallbackChecker : BaseChecker, ICallbackChecker
    {
        private ICallbackChecker _nextChecker;

        public AbstractCallbackChecker(IServiceProvider provider)
            : base(provider)
        {}

        public ICallbackChecker SetNext(ICallbackChecker checker)
        {
            _nextChecker = checker;

            return checker;
        }
        public virtual async Task<object> Checker(CallbackQueryDto incomingCallbackDto)
        {
            if (this._nextChecker != null)
            {
                return await this._nextChecker.Checker(incomingCallbackDto);
            }
            else {
                return null;
            }
        }
    }
}