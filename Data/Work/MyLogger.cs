using System;
using System.Threading.Tasks;
using MyTelegramBot.Data.Interface;

namespace MyTelegramBot.Data.Work
{
    public class MyLogger : IMyLogger
    {
        public readonly IReceiver _receiver;

        public MyLogger(IReceiver receiver)
        {
            _receiver = receiver;
        }

        public void WriteInformation(string info)
        {
            _receiver.Write($"INFO | {DateTime.Today} | {info}");
        }

        public async Task WriteInformationAsync(string info)
        {
            await _receiver.WriteAsync($"\nINFO | {DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss")} | {info}");
        }
    }

}