using System.Collections.Generic;

namespace MyTelegramBot.Models.Telegram
{
    public class InlineKeyboardMarkup
    {
        public InlineKeyboardMarkup(List<List<InlineKeyboardButton>> inline_keyboard)
        {
            this.inline_keyboard = inline_keyboard;
        }
        public List<List<InlineKeyboardButton>> inline_keyboard { get; set; }
    }
}