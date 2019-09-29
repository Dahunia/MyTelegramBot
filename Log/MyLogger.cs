using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Interface;

namespace MyTelegramBot.Log
{
    public class MyLogger<T>: IMyLogger<T>
    {
        public readonly IReceiver _receiver;
        public readonly ILogger<T> _logger;
        public MyLogger(
            IReceiver receiver,
            ILogger<T> logger)
        {
            _receiver = receiver;
            _logger = logger;
        }
        public async Task LogInformation(string message)
        {
            _logger?.LogInformation(message);
            await WriteInformationAsync(message);
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