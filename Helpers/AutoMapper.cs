using AutoMapper;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Models.Telegram;

namespace MyTelegramBot.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<FromDto, User>();
            CreateMap<ChatDto, Chat>();
            CreateMap<MessageDto, Message>();
            CreateMap<UpdateForCreationDto, Update>();
        }
    }
}