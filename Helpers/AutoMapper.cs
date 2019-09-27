using System.Threading.Tasks;
using AutoMapper;
using MyTelegramBot.Data;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Models.Telegram;
using Newtonsoft.Json;

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
            CreateMap<EntityDto, Entity>();
            CreateMap<CallbackQueryDto, CallbackQuery>()
                .ForMember(dest => dest.From, opt => {
                    opt.MapFrom(
                        dto => context.Users.Find(dto.From.Id) != null ? null : dto.From
                    );
                })
                .ForMember(dest => dest.Message, opt => {
                    opt.MapFrom(
                        dto =>  context.Messages.Find(dto.Message.Id) != null ? null : dto.Message
                    );
                })
                .IncludeAllDerived();
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
                })
                .ForMember(dest => dest.ReplyMarkup, opt => {
                    opt.MapFrom(
                        d => JsonConvert.SerializeObject(d.ReplyMarkup)
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