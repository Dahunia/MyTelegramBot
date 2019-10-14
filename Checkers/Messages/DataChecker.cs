using System;
using System.Threading.Tasks;
using AutoMapper;
using MyTelegramBot.Interface;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Models.Telegram;
using Microsoft.Extensions.Logging;

namespace MyTelegramBot.Checkers.Messages
{
    public class DataChecker : AbstractMessageChecker
    {
        private readonly IAuthRepository _authRepository;
        private readonly IDataRepository _dataRepository;
        private readonly IBackwardRepository _backwardRepository;
        private readonly IMapper _mapper;
        public DataChecker(
            IAuthRepository authRepository,
            IDataRepository dataRepository,
            IBackwardRepository backwardRepository,
            IMapper mapper) 
        :base(null, null, null) => 
        (_authRepository, _dataRepository, _backwardRepository, _mapper) =
        (authRepository, dataRepository, backwardRepository, mapper);
        public override async Task<string> Checker(MessageDto incomingMessageDto)
        {
            var user = await _authRepository.GetUser(incomingMessageDto.From.Id);
            if (user == null) 
            {
                var userDto = incomingMessageDto.From;
                var userToCreate = _mapper.Map<User>(userDto);

                user = await _authRepository.Register(userToCreate);
            }
            else { //not created, because when create then automaticaly assign LastActive
                user.LastActive = DateTime.Now;
                await _authRepository.SaveAllAsync();
            }

            if (!await _authRepository.ChatExists(incomingMessageDto.Chat.Id)) 
            {
                var chatDto = incomingMessageDto.Chat;
                var chatToCreate = _mapper.Map<Chat>(chatDto);

                var createdChat = await _authRepository.Register(chatToCreate);
            }

            var response = await base.Checker(incomingMessageDto);

            await _backwardRepository.SetBackwardCommand(user.Id, incomingMessageDto.Text);

            if (!await _dataRepository.MessageExists(incomingMessageDto.Id)) 
            {
                var messageForCreate = _mapper.Map<Message>(incomingMessageDto);

                var createdMessage = _dataRepository.Add(messageForCreate);
                await _dataRepository.SaveAllAsync();
            }
            
            return response;//await base.Checker(incomingMessageDto);
        }
    }
}