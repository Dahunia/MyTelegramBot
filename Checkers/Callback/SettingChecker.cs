using System;
using System.Linq;
using System.Threading.Tasks;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Interface;

namespace MyTelegramBot.Checkers.Callback
{
    public class SettingsChecker : AbstractCallbackChecker
    {
        //private readonly IMyLogger<SettingsChecker> _logger;
        private readonly IBackwardRepository _backwardRepository;
        private readonly ITelegramView _view;
        //private readonly ITelegramRequest _telegramRequest;
         private readonly string[] commands = {@"/about", @"/?"};
        public SettingsChecker(
            IMyLogger<AbstractCallbackChecker> logger,
            ITelegramRequest telegramRequest,
            IBackwardRepository backwardRepository,
            ITelegramView view)
        : base(logger, backwardRepository, telegramRequest)
        { 
            //_logger = logger;
            _backwardRepository = backwardRepository; 
            _view = view;
        }
        public override async Task<string> Checker(CallbackQueryDto incomingCallbackDto)
        {
            var messageTextForEdit = await CheckCallback(incomingCallbackDto);
            return (messageTextForEdit != null) ? 
                await base.SendView(messageTextForEdit, incomingCallbackDto):
                await base.Checker(incomingCallbackDto);
        }

        public async Task<MessageTextForEditDto> CheckCallback(CallbackQueryDto incomingCallbackDto)
        {
            var command = incomingCallbackDto.Data.ToLower();
            var userId = incomingCallbackDto.From.Id;
            MessageTextForEditDto messageTextForEdit = null;

            if (command.Equals(@"/backward")) 
            {
                messageTextForEdit = await _backwardRepository.GetBackwardMessageTextForEditView(userId);
                if (messageTextForEdit == null) 
                {
                    command = await _backwardRepository.GetBackwardCommand(userId);
                    incomingCallbackDto.Data = command;
                }
            }

            if (commands.Contains(command))
            {
                switch (command)
                {   
                    case @"/?": 
                        return await _view.QuestionView(incomingCallbackDto.Message);
                    case @"/about": 
                        return await _view.AboutView(incomingCallbackDto.Message);
                };                   
            }
            return messageTextForEdit;
        }
    }
}
//var response = await _telegramRequest.SendCallback(answerForSend);
/*  var answerForSend= new AnswerCallbackQueryDto 
{
    callback_query_id = incomingCallbackDto.Id,
    text = "Waiting..."
    //Sample Ожидание... или Ваши данные переданы
}; */