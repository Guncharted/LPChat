using LPChat.Domain.Interfaces;
using LPChat.Infrastructure.Services;
using LPChat.Middleware;
using LPChat.MongoDb;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Net;
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
            services.AddScoped<IPersonInfoService, PersonInfoService>();

            services
                .AddSingleton<IRepositoryManager, MongoReposiotoryManager>(x => new MongoReposiotoryManager("cchat", configuration.GetConnectionString("MongoLocal")));

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

		public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<ExceptionHandler>();
		}
    }
}
