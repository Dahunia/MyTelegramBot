using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Interface;
using MyTelegramBot.Views.Telegram;
using Newtonsoft.Json;
using System.Linq;

namespace MyTelegramBot.Checkers.Callback
{
    public class CategoryCallbackChecker : AbstractCallbackChecker
    {
        private readonly IMyLogger<CategoryCallbackChecker> _logger;
        private readonly IDataRepository _dataRepository;
        private readonly ITelegramView _view;
        protected readonly ITelegramApiRequest _telegramRequest;

        private readonly string[] commands = {};
        public CategoryCallbackChecker(
            IMyLogger<CategoryCallbackChecker> logger,
            IDataRepository dataRepository,
            ITelegramView view,
            ITelegramApiRequest telegramRequest)
        { 
            _logger = logger;
            _dataRepository = dataRepository; 
            _view = view;
            _telegramRequest = telegramRequest;

            var categories = _dataRepository.GetAllCategories().Result;
            commands = categories.AsQueryable().Select(c => @"/" + c.Id).ToArray();
        }
        public override async Task<string> Checker(CallbackQueryDto incomingCallbackDto)
        {
            int categoryId;
            if (commands.Contains(incomingCallbackDto.Data))
            {
                categoryId = System.Convert.ToInt16(incomingCallbackDto.Data.Substring(1));
                var answerForSend = await _view.Category(incomingCallbackDto.Message, categoryId);
                var response = await _telegramRequest.ChangeMessage(answerForSend);

                await _logger.LogInformation("RESPONSE TO USER\n" + JsonConvert.SerializeObject(answerForSend));
                return response;
            }
            return await base.Checker(incomingCallbackDto);
        }
    }
}
