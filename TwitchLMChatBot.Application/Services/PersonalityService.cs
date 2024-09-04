using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLMChatBot.Application.Abstractions;
using TwitchLMChatBot.Application.Contracts;
using TwitchLMChatBot.Application.Exceptions;
using TwitchLMChatBot.Models;

namespace TwitchLMChatBot.Application.Services
{
    public class PersonalityService : IPersonalityService
    {
        private readonly IPersonalityRepository _personalityRepository;
        private readonly IValidator<CreatePersonalityRequest> _createValidator;
        private readonly IValidator<UpdatePersonalityRequest> _updateValidator;

        public PersonalityService(IPersonalityRepository personalityRepository,
            IValidator<CreatePersonalityRequest> createValidator,
            IValidator<UpdatePersonalityRequest> updateValidator)
        {
            _personalityRepository = personalityRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public void CreatePersonality(CreatePersonalityRequest request)
        {
            _createValidator.ValidateAndThrow(request);

            var personality = new Personality();
            personality.Name = request.PersonalityName;
            personality.Instructions = request.Instructions;

            _personalityRepository
                .Insert(personality);
        }

        public void SetAsDefault(int personalityId)
        {
            var personality = _personalityRepository.FindById(personalityId);

            if (personality == null)
            {
                throw new PersonalityNotFoundException(personalityId);
            }

            var items = _personalityRepository.Find(a => a.IsDefault);
            foreach (var item in items)
            {
                item.IsDefault = false;
                _personalityRepository
                    .Update(item);
            }

            personality.IsDefault = true;
            _personalityRepository.Update(personality);
        }

        public void UpdatePersonality(int id, UpdatePersonalityRequest request)
        {
            _updateValidator.Validate(request);

            var personality = _personalityRepository.FindById(id);
            personality.Name = request.PersonalityName;
            personality.Instructions = request.Instructions;

            _personalityRepository
                .Update(personality);
        }
    }
}
