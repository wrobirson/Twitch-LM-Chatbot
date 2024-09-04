using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwitchLMChatBot.Application.Abstractions;
using TwitchLMChatBot.Application.Exceptions;
using TwitchLMChatBot.Application.UseCases;
using TwitchLMChatBot.Application.UseCases.Providers;
using TwitchLMChatBot.Application.UseCases.Providers.Create;
using TwitchLMChatBot.Application.UseCases.Providers.SetAsDefault;
using TwitchLMChatBot.Models;
using TwitchLMChatBot.Persistence.Reopsotories;

namespace TwitchLMChatBot.Server.Controllers
{
    [Route("api/providers")]
    [ApiController]
    public class ProvidersController : ControllerBase
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IMediator _mediator;

        public ProvidersController(IProviderRepository providerRepository, IMediator mediator)
        {
            _providerRepository = providerRepository;
            _mediator = mediator;
        }

        [HttpGet(Name =nameof(ListProviders))]
        public ActionResult ListProviders()
        {
            return Ok(_providerRepository.FindAll());
        }

        public record CreateProviderRequest(
            ProviderType ProviderType, 
            string ProviderName, 
            string? BaseUrl, 
            string? ApiKey);

        [HttpPost(Name =nameof(CreateProvider))]
        public async Task<IActionResult> CreateProvider(CreateProviderRequest request)
        {
            try
            {
                var command = new CreateProviderCommand(
                    request.ProviderType,
                    request.ProviderName,
                    request.BaseUrl,
                    request.ApiKey);

                await _mediator.Send(command);
                return Created();
            }
            catch (ValidationException ex)
            {
              return BadRequest(ex.Errors);
            }
        }

        [HttpDelete("{id}", Name =nameof(DeleteProvider))]
        public ActionResult DeleteProvider(int id)
        {
            var provider = _providerRepository.FindById(id);
            if (provider == null)
            {
                return NotFound();
            }
            _providerRepository.Delete(provider);
            return NoContent();
        }

        [HttpPut("{id}/default", Name =nameof(SetDefault))]
        public async Task<IActionResult> SetDefault(int id)
        {
            try
            {
                await _mediator.Send(new SetDefaultProviderCommand(id));
                return NoContent();
            }
            catch (ProviderNotFoundException)
            {
                return NotFound();
            }
        }

    }
}
