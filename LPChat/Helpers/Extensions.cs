using LPChat.Core.Interfaces;
using LPChat.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPChat.Helpers
{
    public static class Extensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IMessageService, MessageService>();
        }
    }
}
