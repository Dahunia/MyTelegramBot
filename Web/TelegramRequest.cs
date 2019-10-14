using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MyTelegramBot.Interface;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Models.Settings;
using MyTelegramBot.Log;
using Microsoft.Extensions.Logging;

namespace MyTelegramBot.Web
{
    public class TelegramRequest : ITelegramRequest
    {  
        private readonly ILoggerFactory _loggerFactory;
        private readonly TelegramSettings _telegramConfig;
        private readonly IReceiver _receiver;
        public TelegramRequest(
            ILoggerFactory loggerFactory,
            IReceiver fileReciever,
            TelegramSettings telegramConfig)
           // IOptions<TelegramSettings> telegramConfig)
        {
            _loggerFactory = loggerFactory;
            _receiver = fileReciever;
            _telegramConfig = telegramConfig;
        }

        public async Task<string> SendMessage<TButton>(MessageForSendDto<TButton> message) 
            where TButton: class
        {
            string url = GetUrl("sendMessage");
            return await SendRequest(url, message);
        }
        public async Task<string> SendCallback(AnswerCallbackQueryDto answerCallbackQuery) 
        {
            string url = GetUrl("answerCallbackQuery");
            return await SendRequest(url, answerCallbackQuery);
        }
        public async Task<string> SendChangeMessage(MessageTextForEditDto messageTextForEdit)
        {
            string url = GetUrl("editMessageText");
            return await SendRequest(url, messageTextForEdit);
        }
        private string GetUrl(string method) {
            return _telegramConfig.BaseEndpoint
                .Replace("<token>", _telegramConfig.Token)
                .Replace("METHOD_NAME", method); 
        }

        private async Task<T> GetRequest<T>(string url) {
            var apiLogger = new MyLogger<ApiGetingData<T>>(
                _receiver,
                _loggerFactory.CreateLogger<ApiGetingData<T>>()
            );
            var getRequest = new ApiGetingData<T>(apiLogger);

            var response = await getRequest.GetDataAsync(
                url,
                _telegramConfig.WithProxy ? 
                    _telegramConfig.Proxies.FirstOrDefault() :
                    null
            );
    
            return response;
        } 

        private async Task<string> SendRequest<T>(string url, T entity) {
            var apiLogger = new MyLogger<ApiGetingData<T>>(
                _receiver,
                _loggerFactory.CreateLogger<ApiGetingData<T>>()
            );
            var sendRequest = new ApiGetingData<T>(apiLogger);

            return await sendRequest.SendDataAsync(
                entity,
                url, 
                _telegramConfig.WithProxy ? 
                    _telegramConfig.Proxies.FirstOrDefault() :
                    null
            );
        }
        public async Task<UpdateForCreationDto> GetUpdate() 
        {
            var url = GetUrl("getUpdates");
            var lastUpdateId = _telegramConfig.LastUpdate;
            var query = HttpUtility.ParseQueryString("");
            
            if (lastUpdateId != 0) {
                query["offset"] = lastUpdateId.ToString();

                url += "?" + query.ToString();
            }

            var update = await GetRequest<UpdateForCreationDto>(url);

            //await _logger.LogInformation(url + "\n");
          /*   
            if (update.result.Count() > 0 ) {
                 _telegramConfig.LastUpdate = update.result.LastOrDefault().UpdateId + 1;
            }
            else { 
                _telegramConfig.LastUpdate = 0;
            } */

            return update;
        }
    }
}