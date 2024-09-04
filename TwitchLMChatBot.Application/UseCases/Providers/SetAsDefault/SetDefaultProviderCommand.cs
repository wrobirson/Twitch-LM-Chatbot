using FluentResults;
using LanguageExt;
using LanguageExt.Pipes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLMChatBot.Application.Abstractions;
using TwitchLMChatBot.Application.Exceptions;

namespace TwitchLMChatBot.Application.UseCases.Providers.SetAsDefault
{
    public class SetDefaultProviderCommand(int providerId) : IRequest
    {
        public int ProviderId { get; } = providerId;
    }

    class SetDefaultProviderCommandHandler : IRequestHandler<SetDefaultProviderCommand>
    {
        private readonly IProviderRepository _providerRepository;

        public SetDefaultProviderCommandHandler(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }
        public async Task Handle(SetDefaultProviderCommand request, CancellationToken cancellationToken)
        {
            var provider = _providerRepository.FindById(request.ProviderId);

            if (provider == null)
            {
                throw new ProviderNotFoundException(request.ProviderId);
            }

            var defaults = _providerRepository.FindAll();
            foreach (var item in defaults)
            {
                item.IsDefault = false;
                _providerRepository.Update(item);
            }

            provider.IsDefault = true;
            _providerRepository.Update(provider);

        }
    }
}
