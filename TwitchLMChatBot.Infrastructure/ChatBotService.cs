using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLMChatBot.Application.Abstractions;

namespace TwitchLMChatBot.Infrastructure
{
    public class ChatBotService : IHostedService, IDisposable
    {
        private readonly IChatBot chatBot;
        private readonly IServiceProvider _serviceProvider;
        private CancellationTokenSource _cts;

        public ChatBotService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            using (var scope = _serviceProvider.CreateScope())
            {
                var chatBot = scope.ServiceProvider.GetRequiredService<IChatBot>();
                chatBot.Connect();
                // Usa chatBot aquí
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            chatBot.Disconnect();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _cts.Dispose();
        }

        public Task RestartAsync()
        {
            StopAsync(CancellationToken.None).Wait();
            return StartAsync(CancellationToken.None);
        }
    }

    public interface IChatBotServiceController
    {
        Task RestartAsync();
    }
}
