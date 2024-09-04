using TwitchLMChatBot.Models;

namespace TwitchLMChatBot.Application.Abstractions
{
    public interface IPersonalityRepository : IRepository<Personality>
    {
        Personality GetDefault();
    }
}