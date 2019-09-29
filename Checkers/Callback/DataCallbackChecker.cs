using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Interface;
using MyTelegramBot.Models.Telegram;

namespace MyTelegramBot.Checkers.Callback
{
    public class DataCallbackChecker : AbstractCallbackChecker
    {
        private readonly IAuthRepository _authRepository;
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        public DataCallbackChecker(
            IAuthRepository authRepository,
            IDataRepository dataRepository,
            IMapper mapper)
        {
            _authRepository = authRepository;
            _dataRepository = dataRepository;
            _mapper = mapper;
        }

        public override async Task<string> Checker(CallbackQueryDto incomingCallbackDto)
        {
            var user = await _authRepository.GetUser(incomingCallbackDto.From.Id);
            if (user == null) 
            {
                var userDto = incomingCallbackDto.From;
                var userToCreate = _mapper.Map<User>(userDto);

                user = await _authRepository.Register(userToCreate);
            }
            else { //not created, because when create then automaticaly assign LastActive
                user.LastActive = System.DateTime.Now;
                await _authRepository.SaveAllAsync();
            }
            var response = await base.Checker(incomingCallbackDto);

            if (!await _dataRepository.CallbackExists(incomingCallbackDto.Id)) 
            {
                var callbackForCreate = _mapper.Map<CallbackQuery>(incomingCallbackDto);

                var createdCallback = _dataRepository.Add(callbackForCreate);
                await _dataRepository.SaveAllAsync();
            }
            return response;
        }
    }
}