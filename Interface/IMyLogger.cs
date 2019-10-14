using System.Threading.Tasks;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Models.Telegram;

namespace MyTelegramBot.Interface
{
    public interface IMyLogger<T>
    {
        void WriteInformation(string info);
        Task WriteInformationAsync(string info);
        Task LogInformation(string message);
        Task LogIncomingRequest(string request);
        Task LogSerializedResponse(object answerForSend);
        Task LogSentToUser<TButton>(MessageForSendDto<TButton> messageDto)
            where TButton: class;
        Task LogSentToUser(MessageTextForEditDto messageDto);
        Task LogResponseFromTelegram(Response response);
        Task LogResponseFromTelegram(ResponseDto response);

    }
}