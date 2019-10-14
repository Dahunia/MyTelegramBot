using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Helpers;
using MyTelegramBot.Interface;
using MyTelegramBot.Models.Telegram;
using Newtonsoft.Json;

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
        public async Task LogIncomingRequest(string request)
        {
            var requestObject = JsonConvert.DeserializeObject(request);
            await LogInformation("INCOMING REQUEST\n" + requestObject.GetDump());
        }
        public async Task LogSerializedResponse(object answerForSend)
        {
            await LogInformation(
                "RESPONSE serialized data\n" + 
                JsonConvert.SerializeObject(answerForSend));
        }
        public async Task LogResponseFromTelegram(Response response)
        {
             var test = response.GetDump();
            await LogInformation(
                "RESPONSE FROM TELEGRAM\n" + response.GetDump());
        }
        public async Task LogResponseFromTelegram(ResponseDto responseDto)
        {
            var test = responseDto.GetDump();
            await LogInformation(
                "RESPONSE DTO FROM TELEGRAM\n" + responseDto.GetDump());
        }
        public async Task LogSentToUser<TButton>(MessageForSendDto<TButton> messageDto) 
            where TButton: class
        {
            await LogInformation("Was SENT TO USER\n" + messageDto.GetDump());
        }
        public async Task LogSentToUser(MessageTextForEditDto messageDto)
        {
            await LogInformation("Was SENT TO USER\n" + messageDto.GetDump());
        }
        public async Task LogRecievedResponse(ResponseDto responseDto)
        {
            await LogInformation($"\nReceived RESPONSE after send \n {responseDto.GetDump()}");
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