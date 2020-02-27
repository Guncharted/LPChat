using AutoMapper;
using LPChat.Common.Models;
using LPChat.Data.MongoDb.Entities;

namespace LPChat.Services.Mapping
{
    public static class DataMapper
    {
        private static IMapper mapper = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.AddProfile<DataProfile>();
            }).CreateMapper();

        public static D Map<S, D>(S source)
        {
            return mapper.Map<S, D>(source);
        }
    }

    public class DataProfile : Profile
    {
        public DataProfile()
        {
            CreateMap<UserModel, User>()
                .ForMember(d => d.Email, opt => opt.Ignore())
                .ForMember(d => d.PasswordHash, opt => opt.Ignore())
                .ForMember(d => d.PasswordSalt, opt => opt.Ignore())
                .ForMember(d => d.CreatedUtcDate, opt => opt.Ignore())
                .ForMember(d => d.LastUpdatedUtcDate, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<UserSecurityModel, User>()
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email.ToLower()))
                .ForMember(d => d.PasswordHash, opt => opt.Ignore())
                .ForMember(d => d.PasswordSalt, opt => opt.Ignore())
                .ForMember(d => d.CreatedUtcDate, opt => opt.Ignore())
                .ForMember(d => d.LastUpdatedUtcDate, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<MessageModel, Message>().ReverseMap();
            CreateMap<ChatModel, Chat>().ReverseMap();
        }
    }
}
