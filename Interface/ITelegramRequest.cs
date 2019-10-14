using System.Threading.Tasks;
using MyTelegramBot.Dtos.Telegram;

namespace MyTelegramBot.Interface
{
    public interface ITelegramRequest
    {
        //Task<UpdateForCreationDto> GetUpdate();
        Task<string> SendMessage<TButton>(MessageForSendDto<TButton> message) where TButton: class;
        Task<string> SendCallback(AnswerCallbackQueryDto answerCallbackQuery);
        Task<string> SendChangeMessage(MessageTextForEditDto messageTextForEdit);
    }
}