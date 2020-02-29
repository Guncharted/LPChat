using AutoMapper;
using LPChat.Common;
using LPChat.Common.DbContracts;
using LPChat.Services.Interfaces;
using LPChat.Services.Mapping;
using LPChat.Services;
using LPChat.Middleware;
using LPChat.MongoDb;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using LPChat.Filters;
using LPChat.Infrastructure;

namespace LPChat.Services
{
    public static class ConfigurationExtensions
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<UserAuthFilter>();

            services.AddScoped<IAuthorizationContext, AuthorizationContext>();
            services.AddScoped<IUserPolicyService, UserPolicyService>();

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.AddProfile<MainProfile>();
            }).CreateMapper();

            services.AddSingleton(mapper);

            services.AddSingleton<IInstantMessagingService, InstantMessagingService>();

            services
                .AddSingleton<IRepositoryManager, MongoRepositoryManager>(x => new MongoRepositoryManager("cchat", configuration.GetConnectionString("MongoFromDocker")));

            services.AddCors();
            services.AddMemoryCache();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "LP Chat", Version = "v1" });
                c.DocInclusionPredicate((doc, api) => true);
                c.CustomSchemaIds(x => x.FullName);
                c.OperationFilter<SwaggerHeaders>();
                var xmlDocFile = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                if (File.Exists(xmlDocFile))
                {
                    c.IncludeXmlComments(xmlDocFile);
                }
            });

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

            services.AddAuthorization();

            services.AddControllers(opt => opt.Filters.Add(typeof(UserAuthFilter)))
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                });
            services.AddRouting();
        }

        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandler>();
        }
    }
}
