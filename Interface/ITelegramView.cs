using System.Threading.Tasks;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Models.Telegram;

namespace MyTelegramBot.Interface
{
    public interface ITelegramView
    {
        Task<MessageTextForEditDto> Category(MessageDto message, int categoryId);
        Task<MessageTextForEditDto> Question(MessageDto message);
        Task<MessageTextForEditDto> About(MessageDto message);
        //Task<MessageForSendDto<object>> CreateMessageForSend(MessageDto message);
        Task<MessageForSendDto<InlineKeyboardMarkup>> Start(MessageDto messageDto);
        Task<MessageForSendDto<InlineKeyboardMarkup>> Default(MessageDto messageDto);
    }
}