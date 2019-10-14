using System.Threading.Tasks;
using MyTelegramBot.Dtos.Telegram;

namespace MyTelegramBot.Interface
{
    public interface IBackwardRepository
    {
        Task SetBackwardCommand(long userId, string command);
        Task SetBackwardView(long userId, MessageTextForEditDto messageDto);
        
        Task SetBackwardView<TButton>(long userId, MessageForSendDto<TButton> messageDto) 
            where TButton: class;

        Task<string> GetBackwardCommand(long userId);
        Task<MessageTextForEditDto> GetBackwardMessageTextForEditView(long userId);
        Task<MessageForSendDto<T>> GetBackwardMessageForSendView<T>(long userId) 
            where T: class;
        Task<object> GetView(long userId);
    }
}