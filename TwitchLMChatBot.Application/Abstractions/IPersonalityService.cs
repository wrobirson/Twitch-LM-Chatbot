using TwitchLMChatBot.Application.Contracts;

namespace TwitchLMChatBot.Application.Abstractions
{
    public interface IPersonalityService
    {
        void CreatePersonality(CreatePersonalityRequest request);

        void SetAsDefault(int personalityId);

        void UpdatePersonality(int id, UpdatePersonalityRequest request);
    }
}