using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwitchLMChatBot.Application.Abstractions;
using TwitchLMChatBot.Application.Contracts;
using TwitchLMChatBot.Application.Exceptions;
using TwitchLMChatBot.Models;
using TwitchLMChatBot.Persistence.Reopsotories;

namespace TwitchLMChatBot.Server.Controllers
{
    [Route("api/personalities")]
    [ApiController]
    public class PersonalitiesController : ControllerBase
    {
        private readonly IPersonalityService _personalityService;
        private readonly IPersonalityRepository _personalityRepository;

        public PersonalitiesController(
            IPersonalityService personalityService,
            IPersonalityRepository personalityRepository)
        {
            _personalityService = personalityService;
            _personalityRepository = personalityRepository;
        }

        [HttpGet(Name =nameof(GetPersonalities))]
        [ProducesResponseType<Personality[]>(StatusCodes.Status200OK)]
        public IActionResult GetPersonalities()
        {
            return Ok(_personalityRepository.FindAll());
        }

        [HttpPut("{id}/default", Name =nameof(SetDefaultPersonality))]
        public IActionResult SetDefaultPersonality(int id)
        {
            try
            {
                _personalityService.SetAsDefault(id);
                return NoContent();
            }
            catch (PersonalityNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost(Name =nameof(CreatePersonality))]
        public IActionResult CreatePersonality(CreatePersonalityRequest request)
        {
            try
            {
                _personalityService.CreatePersonality(request);
                return Created();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex);
            }
        }
       

        [HttpPut("{id}", Name =nameof(UpdatePersonality))]
        public  IActionResult UpdatePersonality(int id, UpdatePersonalityRequest request)
        {
            _personalityService.UpdatePersonality(id, request);
         
            return NoContent();
        }

        [HttpDelete("{id}", Name = nameof(DeletePersonality))]
        public IActionResult DeletePersonality(int id)
        {
            var personality = _personalityRepository.FindById(id);
            _personalityRepository.Delete(personality);
            return NoContent();
        }

    }
}
