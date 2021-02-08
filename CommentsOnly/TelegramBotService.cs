using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace CommentsOnly
{
    internal class TelegramBotService : BackgroundService
    {
        private readonly ITelegramBotClient _client;
        private readonly IUpdatesHandler _handler;

        public TelegramBotService(
            ITelegramBotClient client,
            IUpdatesHandler handler)
        {
            _client = client;
            _handler = handler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            IAsyncEnumerable<Update> updates = GetUpdates();

            _client.StartReceiving(cancellationToken: stoppingToken);
            
            await foreach (Update update in updates.WithCancellation(stoppingToken))
            {
                Task.Run(
                    () => _handler.HandleAsync(update, stoppingToken),
                    stoppingToken);
            }
        }

        private IAsyncEnumerable<Update> GetUpdates()
        {
            return Observable
                .FromEventPattern<UpdateEventArgs>(
                    action => _client.OnUpdate += action,
                    action => _client.OnUpdate -= action)
                .Select(pattern => pattern.EventArgs.Update)
                .ToAsyncEnumerable();
        }
    }
}