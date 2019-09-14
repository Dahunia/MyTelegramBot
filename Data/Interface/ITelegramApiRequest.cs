using System.Threading.Tasks;
using MyTelegramBot.Dtos.Telegram;

namespace MyTelegramBot.Data.Work.Interface
{
    public interface ITelegramApiRequest
    {
        //Task<UpdateForCreationDto> GetUpdate();
        Task<byte[]> SendMessage(MessageForSendDto message);
        Task<byte[]> SendCallback(AnswerCallbackQueryDto answerCallbackQuery);
    }
}