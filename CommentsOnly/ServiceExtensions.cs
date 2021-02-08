using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace CommentsOnly
{
    internal static class ServiceExtensions
    {
        public static IServiceCollection AddTelegramBotClient(this IServiceCollection services)
        {
            static ITelegramBotClient Create(IServiceProvider provider)
            {
                var config = provider.GetRequiredService<IConfiguration>();
                var token = config.GetSection("Telegram:Token").Value;

                return new TelegramBotClient(token);
            }

            return services.AddSingleton(Create);
        }
        
        public static IServiceCollection AddAllOfType(this IServiceCollection services, Type type)
        {
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => type.IsAssignableFrom(t) && t.IsClass)
                .ToList()
                .ForEach(t => services.AddSingleton(type, t));

            return services;
        }
    }
}