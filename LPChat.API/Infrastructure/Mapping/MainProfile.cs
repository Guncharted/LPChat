﻿using AutoMapper;
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

            CreateMap<UserInfoViewModel, UserModel>()
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email.ToLower()))
                .ReverseMap();
            CreateMap<UserLoginViewModel, UserSecurityModel>()
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email.ToLower()))
                .ReverseMap();
            CreateMap<UserPasswordChangeViewModel, UserSecurityModel>()
                .ForMember(d => d.Password, opt => opt.MapFrom(s => s.NewPassword))
                .ForMember(d => d.ConfirmPassword, opt => opt.MapFrom(s => s.ConfirmNewPassword))
                .ReverseMap();
            CreateMap<UserRegisterViewModel, UserSecurityModel>()
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email.ToLower()))
                .ReverseMap();
            CreateMap<MessageViewModel, MessageModel>().ReverseMap();
        }
    }
}
