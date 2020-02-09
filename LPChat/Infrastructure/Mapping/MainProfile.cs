using AutoMapper;
using LPChat.Infrastructure.Models;
using LPChat.Infrastructure.ViewModels;

namespace LPChat.Infrastructure.Mapping
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            AllowNullCollections = true;

            CreateMap<ChatCreateViewModel, ChatModel>().ReverseMap();
            CreateMap<ChatInfoViewModel, ChatModel>().ReverseMap();
            CreateMap<ChatStateViewModel, ChatModel>().ReverseMap();

            CreateMap<PersonInfoViewModel, UserModel>().ReverseMap();
            CreateMap<PersonLoginViewModel, UserSecurityModel>().ReverseMap();
            CreateMap<PersonPasswordChangeViewModel, UserSecurityModel>().ReverseMap();
            CreateMap<PersonRegisterViewModel, UserSecurityModel>().ReverseMap();

            CreateMap<MessageViewModel, MessageModel>().ReverseMap();
        }
    }
}
