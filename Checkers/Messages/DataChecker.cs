using System;
using System.Threading.Tasks;
using AutoMapper;
using MyTelegramBot.Data.Interface;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Models.Telegram;

namespace MyTelegramBot.Checkers.Messages
{
    public class DataChecker : AbstractMessageChecker
    {
        private readonly IAuthRepository _repo;
        private readonly IMapper _mapper;
        public DataChecker(
            IAuthRepository repo,
            IServiceProvider provider,
            IMapper mapper)
            : base(provider) => (_repo, _mapper) = (repo, mapper);

        public override async Task<object> Checker(MessageDto incomingMessageDto)
        {
            if (!await _repo.UserExists(incomingMessageDto.From.Id)) {
                var userDto = incomingMessageDto.From;
                var userToCreate = _mapper.Map<User>(userDto);

                var createdUser = await _repo.Register(userToCreate);
            }

            if (!await _repo.UserExists(incomingMessageDto.Chat.Id)) {
                var chatDto = incomingMessageDto.Chat;
                var chatToCreate = _mapper.Map<Chat>(chatDto);

                var createdChat = await _repo.Register(chatToCreate);
            }
            
            return base.Checker(incomingMessageDto);
        }
    }
}