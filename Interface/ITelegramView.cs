using System.Threading.Tasks;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Models.Telegram;

namespace MyTelegramBot.Interface
{
    public interface ITelegramView
    {
        Task<MessageTextForEditDto> CategoriesForEditView(MessageDto message, int categoryId);
        Task<MessageTextForEditDto> ProductsForEditView(MessageDto message, int categoryId);
        Task<MessageTextForEditDto> QuestionView(MessageDto message);
        Task<MessageTextForEditDto> AboutView(MessageDto message);
        Task<MessageTextForEditDto> DefaultCallbackView(MessageDto message);
        //Task<MessageForSendDto<object>> CreateMessageForSend(MessageDto message);
        Task<MessageForSendDto<InlineKeyboardMarkup>> StartView(MessageDto messageDto);
        Task<MessageForSendDto<InlineKeyboardMarkup>> DefaultView(MessageDto messageDto);
    }
}