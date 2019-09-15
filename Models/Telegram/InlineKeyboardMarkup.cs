using System.Collections.Generic;

namespace MyTelegramBot.Models.Telegram
{
    public class InlineKeyboardMarkup
    {
        public InlineKeyboardMarkup(List<List<InlineKeyboardButton>> inline_keyboard)
        {
            this.inline_keyboard = inline_keyboard;
        }
        public InlineKeyboardMarkup() =>
            this.inline_keyboard = new List<List<InlineKeyboardButton>>();
        
        public List<List<InlineKeyboardButton>> inline_keyboard { get; set; }
        public void AddButton(InlineKeyboardButton button) =>
            this.inline_keyboard.Add(new List<InlineKeyboardButton> { button });
        public void AddLineButtons(List<InlineKeyboardButton> lineButtons) =>
            this.inline_keyboard.Add(lineButtons);
        public void AddButton(InlineKeyboardButton button, int numLine)
        {
            while(inline_keyboard.Count <= numLine)
            {
                inline_keyboard.Add(new List<InlineKeyboardButton>());
            }
            inline_keyboard[numLine].Add(button);
        }
    }
}