using LPChat.Core.Entities;
using LPChat.Core.Interfaces;
using LPChat.Infrastructure.Repositories;
using LPChat.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LPChat.Helpers
{
	public static class Extensions
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMessageService, MessageService>();
            services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IChatService, ChatService>();

            //DB collections
            services
                .AddScoped(x => new MongoDbService<Person>("lpchat", "persons", configuration.GetConnectionString("MongoLocal")));
            services
                .AddScoped(x => new MongoDbService<Chat>("lpchat", "chats", configuration.GetConnectionString("MongoLocal")));
            services
                .AddScoped(x => new MongoDbService<Message>("lpchat", "messages", configuration.GetConnectionString("MongoLocal")));

            services.AddCors();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }
    }
}
