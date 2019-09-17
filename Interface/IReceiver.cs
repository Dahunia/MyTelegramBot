using System.Threading.Tasks;

namespace MyTelegramBot.Interface
{
    public interface IReceiver {
        void Write(string info);
        Task WriteAsync(string info);
        string Read();
    }
}