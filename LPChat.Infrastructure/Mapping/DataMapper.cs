using AutoMapper;
using LPChat.Common.Models;
using LPChat.Data.MongoDb.Entities;

namespace LPChat.Infrastructure.Mapping
{
    public static class DataMapper
    {
        private static readonly IMapper mapper;

        static DataMapper()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.AddProfile<DataProfile>();
            });
        }

        public static D Map<S, D>(S source, D destination)
        {
            return mapper.Map<S, D>(source);
        }
    }

    public class DataProfile : Profile
    {
        public DataProfile()
        {
            CreateMap<UserModel, User>().ReverseMap();
            CreateMap<UserSecurityModel, User>().ReverseMap();
            CreateMap<MessageModel, Message>().ReverseMap();
            CreateMap<ChatModel, Chat>().ReverseMap();
        }
    }
}
