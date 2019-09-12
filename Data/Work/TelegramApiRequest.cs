using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MyTelegramBot.Data.Work.Interface;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Models.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyTelegramBot.Data.Interface;

namespace MyTelegramBot.Data.Work
{
    public class TelegramApiRequest : ITelegramApiRequest
    {
        private readonly TelegramSettings _telegramConfig;
        private readonly ILogger<TelegramApiRequest> _logger;
        private readonly IMyLogger _filelogger;
        public TelegramApiRequest(
            IOptions<TelegramSettings> telegramConfig,
            ILogger<TelegramApiRequest> logger)
        {
            _telegramConfig = telegramConfig.Value;
            _logger = logger;
            _filelogger = _filelogger;
        }

        public async Task<byte[]> SendMessage(MessageForSendDto message) 
        {
            string url = GetUrl("sendMessage");
            return await SendRequest(url, message);
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

            _logger.LogInformation(url + "\n");
            
            if (update.result.Count() > 0 ) {
                 _telegramConfig.LastUpdate = update.result.LastOrDefault().update_id + 1;
            }
            else { 
                _telegramConfig.LastUpdate = 0;
            }

            return update;
        }   
        private string GetUrl(string method) {
            return _telegramConfig.BaseEndpoint
                .Replace("<token>", _telegramConfig.Token)
                .Replace("METHOD_NAME", method); 
        }

        private async Task<T> GetRequest<T>(string url) {
            
            var getRequest = new ApiGetingData<T>(_logger, _filelogger);

            var response = await getRequest.GetDataAsync(
                url,
                _telegramConfig.Proxies.First()
            );
    
            return response;
        } 

        private async Task<byte[]> SendRequest<T>(string url, T entity) {

            var sendRequest = new ApiGetingData<T>(_logger, _filelogger);

            return await sendRequest.SendDataAsync(
                entity,
                url, 
                _telegramConfig.WithProxy ? 
                    _telegramConfig.Proxies.FirstOrDefault() :
                    null
            );
        }
        public async Task<byte[]> AnswerCallbackQuery(AnswerCallbackQueryDto answerCallbackQuery) 
        {
            string url = GetUrl("answerCallbackQuery");
            return await SendRequest(url, answerCallbackQuery);
        }
    }
}