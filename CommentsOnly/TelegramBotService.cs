using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace CommentsOnly
{
    internal class TelegramBotService : BackgroundService
    {
        private readonly ITelegramBotClient _client;
        private readonly IUpdatesHandler _handler;
        private readonly ILogger<TelegramBotService> _logger;

        public TelegramBotService(
            ITelegramBotClient client,
            IUpdatesHandler handler,
            ILogger<TelegramBotService> logger)
        {
            _client = client;
            _handler = handler;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            IAsyncEnumerable<Update> updates = GetUpdates();

            _client.StartReceiving(cancellationToken: stoppingToken);
            
            await foreach (Update update in updates.WithCancellation(stoppingToken))
            {
                async Task Handle()
                {
                    try
                    {
                        await _handler.HandleAsync(update, stoppingToken);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Failed to handle update");
                    }
                }

                Task.Run(
                    Handle,
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