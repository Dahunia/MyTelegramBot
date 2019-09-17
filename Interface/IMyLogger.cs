using System.Threading.Tasks;

namespace MyTelegramBot.Interface
{
    public interface IMyLogger
    {
        void WriteInformation(string info);
        Task WriteInformationAsync(string info);
    }
}