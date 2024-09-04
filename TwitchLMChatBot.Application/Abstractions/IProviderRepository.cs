using TwitchLMChatBot.Models;

namespace TwitchLMChatBot.Application.Abstractions
{
    public interface IProviderRepository : IRepository<Provider>
    {
        public Provider GetByType(ProviderType type);
        Provider GetDefault();
    }
}
