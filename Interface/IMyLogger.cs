using System.Threading.Tasks;

namespace MyTelegramBot.Interface
{
    public interface IMyLogger<T>
    {
        void WriteInformation(string info);
        Task WriteInformationAsync(string info);
        Task LogInformation(string message);
    }
}