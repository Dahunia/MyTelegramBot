using System;
using System.Threading.Tasks;
using AutoMapper;
using MyTelegramBot.Interface;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Models.Telegram;

namespace MyTelegramBot.Checkers.Messages
{
    public class DataChecker : AbstractMessageChecker
    {
        private readonly IAuthRepository _repo;
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        public DataChecker(
            IAuthRepository repo,
            IDataRepository dataRepository,
            IServiceProvider provider,
            IMapper mapper)
            : base(provider) => (_repo, _dataRepository, _mapper) = (repo, dataRepository, mapper);

        public override async Task<string> Checker(MessageDto incomingMessageDto)
        {
            var user = await _repo.GetUser(incomingMessageDto.From.Id);
            if (user == null) 
            {
                var userDto = incomingMessageDto.From;
                var userToCreate = _mapper.Map<User>(userDto);

                var createdUser = await _repo.Register(userToCreate);
            }
            else {
                user.LastActive = DateTime.Now;
            }

            if (!await _repo.ChatExists(incomingMessageDto.Chat.Id)) 
            {
                var chatDto = incomingMessageDto.Chat;
                var chatToCreate = _mapper.Map<Chat>(chatDto);

                var createdChat = await _repo.Register(chatToCreate);
            }

            if (!await _dataRepository.MessageExists(incomingMessageDto.MessageId)) 
            {
                var messageForCreate = _mapper.Map<Message>(incomingMessageDto);

                var createdMessage = _dataRepository.Add(messageForCreate);
                await _dataRepository.SaveAll();
            }
            
            return await base.Checker(incomingMessageDto);
        }
    }
}