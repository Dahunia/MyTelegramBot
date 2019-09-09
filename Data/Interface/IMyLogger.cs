using System.Threading.Tasks;

namespace MyTelegramBot.Data.Interface
{
    public interface IMyLogger
    {
        void WriteInformation(string info);
        Task WriteInformationAsync(string info);
    }
}