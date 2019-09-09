using System.Threading.Tasks;
using MyTelegramBot.Dtos;

namespace MyTelegramBot.Data.Work.Interface
{
    public interface ITelegramApiRequest
    {
        //Task<UpdateForCreationDto> GetUpdate();
        Task<byte[]> SendMessage(MessageForSendDto message);
    }
}