using System.Threading.Tasks;
using AutoMapper;
using MyTelegramBot.Data;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Models.Telegram;

namespace MyTelegramBot.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        private readonly DataContext _context;
        public AutoMapperProfiles(DataContext context)
        {
            _context = context;
            CreateMap<FromDto, User>();
            CreateMap<ChatDto, Chat>();
            CreateMap<ResponseDto, Response>()
                .IncludeAllDerived();

            CreateMap<UpdateForCreationDto, Update>()
                .ForMember(dest => dest.Message, opt => {
                    opt.MapFrom(
                        udto =>  context.Messages.Find(udto.Message.Id) != null ? null : udto.Message
                    );
                })
                .IncludeAllDerived();

            CreateMap<MessageDto, Message>()
                .ForMember(dest => dest.Date, opt => {
                    opt.MapFrom(d => d.Date.CalculateDateTime());
                })
                .ForMember(dest => dest.Chat, opt => {
                    opt.MapFrom(
                        mdto => context.Chats.Find(mdto.Chat.Id) != null ? null : mdto.Chat
                    );
                })
                .ForMember(dest => dest.From, opt => {
                    opt.MapFrom(
                        mdto => context.Users.Find(mdto.From.Id) != null ? null : mdto.From
                    );
                });

            CreateMap<ResultDto, Result>()
                .ForMember(dest => dest.Date, opt => {
                    opt.MapFrom(d => d.Date.CalculateDateTime());
                })
                .ForMember(dest => dest.Chat, opt => {
                    opt.MapFrom(
                        mdto => context.Chats.Find(mdto.Chat.Id) != null ? null : mdto.Chat
                    );
                })
                .ForMember(dest => dest.From, opt => {
                    opt.MapFrom(
                        mdto => context.Users.Find(mdto.From.Id) != null ? null : mdto.From
                    );
                });
        }

        private async Task<T> Exists<T>(T entity, long id)
            where T : class
        {
            if (await _context.FindAsync(typeof(T), id) != null)
                return null;
            return entity;
        }
    }
}