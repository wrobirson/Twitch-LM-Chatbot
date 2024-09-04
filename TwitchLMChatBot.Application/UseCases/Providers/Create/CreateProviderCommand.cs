using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLMChatBot.Application.Abstractions;
using TwitchLMChatBot.Models;

namespace TwitchLMChatBot.Application.UseCases.Providers.Create
{
    public class CreateProviderCommand : IRequest
    {
        public ProviderType ProviderType { get; }

        public string ProviderName { get; }

        public string? BaseUrl { get; }

        public string? ApiKey { get; }

        public CreateProviderCommand(ProviderType providerType, string providerName, string? baseUrl, string? apiKey)
        {
            ProviderType = providerType;
            ProviderName = providerName;
            BaseUrl = baseUrl;
            ApiKey = apiKey;
        }
    }


    class CreateProviderCommandHandler : IRequestHandler<CreateProviderCommand>
    {
        private readonly IProviderRepository _providerRepository;

        public CreateProviderCommandHandler(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }

        public async Task Handle(CreateProviderCommand request, CancellationToken cancellationToken)
        {
            bool isDefault = _providerRepository.Count() == 0;

            var provider = new Provider();
            provider.Name = request.ProviderName;
            provider.Type = request.ProviderType;
            provider.ApiKey = request.ApiKey;
            provider.BaseUrl = request.BaseUrl;
            provider.IsDefault = isDefault;
            _providerRepository.Insert(provider);
        }
    }
}
