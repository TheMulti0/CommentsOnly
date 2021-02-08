using System;
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
    }
}