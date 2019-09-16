using System.Threading.Tasks;
using MyTelegramBot.Dtos.Telegram;

namespace MyTelegramBot.Data.Interface
{
    public interface ITelegramApiRequest
    {
        //Task<UpdateForCreationDto> GetUpdate();
        Task<string> SendMessage<TButton>(MessageForSendDto<TButton> message) where TButton: class;
        Task<string> SendCallback(AnswerCallbackQueryDto answerCallbackQuery);
    }
}