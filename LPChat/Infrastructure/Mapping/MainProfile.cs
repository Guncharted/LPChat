using AutoMapper;
using LPChat.Common.Models;
using LPChat.Services.ViewModels;

namespace LPChat.Services.Mapping
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            AllowNullCollections = true;

            CreateMap<ChatCreateViewModel, ChatModel>().ReverseMap();
            CreateMap<ChatInfoViewModel, ChatModel>().ReverseMap();
            CreateMap<ChatStateViewModel, ChatModel>().ReverseMap();

            CreateMap<UserInfoViewModel, UserModel>().ReverseMap();
            CreateMap<UserLoginViewModel, UserSecurityModel>().ReverseMap();
            CreateMap<UserPasswordChangeViewModel, UserSecurityModel>().ReverseMap();
            CreateMap<UserRegisterViewModel, UserSecurityModel>().ReverseMap();

            CreateMap<MessageViewModel, MessageModel>().ReverseMap();
        }
    }
}
