using System.Threading.Tasks;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Interface;
using Newtonsoft.Json;
using System.Linq;

namespace MyTelegramBot.Checkers.Callback
{
    public class CategoryCallbackChecker : AbstractCallbackChecker
    {
        //private readonly IMyLogger<CategoryCallbackChecker> _logger;
        private readonly IDataRepository _dataRepository;
        private readonly ITelegramView _view;
        //protected readonly ITelegramRequest _telegramRequest;

        private readonly string[] parentCategories = {};
        private readonly string[] productCategories = {};
        public CategoryCallbackChecker(
            IMyLogger<AbstractCallbackChecker> logger,
            IBackwardRepository backwardRepository,
            IDataRepository dataRepository,
            ITelegramView view,
            ITelegramRequest telegramRequest)
        : base(logger, backwardRepository, telegramRequest)
        { 
            //_logger = logger;
            _dataRepository = dataRepository; 
            _view = view;
            //_telegramRequest = telegramRequest;

            var categories = _dataRepository.GetAllCategories().Result;
            parentCategories = categories.Select(c => @"/cat" + c.Parent).Distinct().ToArray();
            productCategories = categories.Select(c => @"/cat" + c.Id).Except(parentCategories).ToArray();
        }
        public override async Task<string> Checker(CallbackQueryDto incomingCallbackDto)
        {
            var answerForSend = await CheckCallback(incomingCallbackDto);
            return (answerForSend != null) ? 
                await base.SendView(answerForSend, incomingCallbackDto):
                await base.Checker(incomingCallbackDto);
        }  
        public async Task<MessageTextForEditDto> CheckCallback(CallbackQueryDto inCallbackDto)
        {
            int categoryId;
            var userId = inCallbackDto.From.Id;
            // if categories
            if (parentCategories.Contains(inCallbackDto.Data) 
                && TryParse(inCallbackDto.Data, out categoryId))
            {
                return await _view.CategoriesForEditView(inCallbackDto.Message, categoryId);
            }
            // if products
            else if(productCategories.Contains(inCallbackDto.Data)
                && TryParse(inCallbackDto.Data, out categoryId))
            {
                return await _view.ProductsForEditView(inCallbackDto.Message, categoryId);
            }
            // default 
            else {
                return await _view.DefaultCallbackView(inCallbackDto.Message);
            }
        }
        private bool TryParse(string command, out int categoryId)
        {
            command = command.Replace(@"/cat", "");
            return System.Int32.TryParse(command, out categoryId);
        }
    }
}
