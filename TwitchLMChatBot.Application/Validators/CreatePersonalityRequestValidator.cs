using FluentValidation;
using TwitchLMChatBot.Application.Contracts;

namespace TwitchLMChatBot.Application.Validators
{
    public class CreatePersonalityRequestValidator : AbstractValidator<CreatePersonalityRequest>
    {
        public CreatePersonalityRequestValidator()
        {
            RuleFor(a => a.PersonalityName).NotEmpty();
            RuleFor(a => a.Instructions).NotEmpty();
        }
    }
}