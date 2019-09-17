using System;
using System.Threading.Tasks;
using MyTelegramBot.Dtos.Telegram;

namespace MyTelegramBot.Checkers.Messages
{
    public class AbstractMessageChecker : BaseChecker, IMessageChecker
    {
        private IMessageChecker _nextChecker;

        public AbstractMessageChecker(IServiceProvider provider)
            : base(provider)
        {}

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
        }/* 
        private async Task<MessageForSendDto<InlineKeyboardMarkup>> CreateMessageForSend(MessageDto message)
        {
             var messageForSend = new MessageForSendDto<InlineKeyboardMarkup>() {
                chat_id = message.Chat.Id
            };
            switch (message.Text.ToLower()) {
                case "/cat":
                    messageForSend.text = "Категории";
                    
                    var inlineKeyboard = new InlineKeyboardMarkup();
                    
                    //messageForSend.reply_markup = 
                    //    GetInlineButtons(message.chat.id);
                    break;
            }
            return messageForSend;
        } */
    }
}