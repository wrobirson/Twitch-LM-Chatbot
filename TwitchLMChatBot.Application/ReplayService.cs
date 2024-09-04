using Ardalis.GuardClauses;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using TwitchLMChatBot.Application.Abstractions;
using TwitchLMChatBot.Models;

namespace TwitchLMChatBot.Application
{
    public class ReplayService : IReplyService
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IPersonalityRepository _personalityRepository;

        public ReplayService(IProviderRepository providerRepository, IPersonalityRepository personalityRepository)
        {
            _providerRepository = providerRepository;
            _personalityRepository = personalityRepository;
        }

        public string GetResponse(string message)
        {
            //Guard.Against.Null(personality);
            //Guard.Against.Null(personality.Provider);
            //Guard.Against.NegativeOrZero(personality.Provider.Id);

            var personality = _personalityRepository.GetDefault();
            var provider = _providerRepository.GetDefault();

            var options = new OpenAIClientOptions();
            var model = string.Empty;
            var apiKey = string.Empty;
            if (provider.Type == ProviderType.LMStudio)
            {
                apiKey = "NA";
                model = "NA";
                options.Endpoint = new Uri(provider.BaseUrl);
            }

            if (provider.Type == ProviderType.OpenAi)
            {
                apiKey = provider.ApiKey;
                model = "gpt-4o";
            }

            var response = new OpenAIClient(new ApiKeyCredential(apiKey), options)
                .GetChatClient(model)
                .CompleteChat([
                    ChatMessage.CreateSystemMessage(personality.Instructions),
                    ChatMessage.CreateUserMessage(message)
                 ]);

            return response.Value.Content.First().Text;
        }
    }

}
