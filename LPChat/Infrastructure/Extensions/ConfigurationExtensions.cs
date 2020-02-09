using LPChat.Infrastructure.Interfaces;
using LPChat.Infrastructure.Services;
using LPChat.Middleware;
using LPChat.MongoDb;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace LPChat.Infrastructure
{
	public static class ConfigurationExtensions
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
            services.AddMemoryCache();

			//services.AddSwaggerGen(c =>
			//{
			//	c.SwaggerDoc("v1", Microsoft.OpenApi.ope { Title = "LP Chat", Version = "v1" });
			//	c.DescribeAllEnumsAsStrings();
			//	c.DocInclusionPredicate((doc, api) => true);
			//	c.CustomSchemaIds(x => x.FullName);
			//	var xmlDocFile = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
			//	if (File.Exists(xmlDocFile))
			//	{
			//		c.IncludeXmlComments(xmlDocFile);
			//	}
			//});

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

			services.AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
		}

		public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<ExceptionHandler>();
		}
	}
}
