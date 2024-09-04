using FluentValidation;
using TwitchLMChatBot.Application.Contracts;

namespace TwitchLMChatBot.Application.Validators
{
    public class UpddatePersonalityRequestValidator : AbstractValidator<UpdatePersonalityRequest>
    {
        public UpddatePersonalityRequestValidator()
        {
            RuleFor(a => a.PersonalityName).NotEmpty();
            RuleFor(a => a.Instructions).NotEmpty();
        }
    }
}