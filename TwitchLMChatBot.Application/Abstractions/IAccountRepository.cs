using TwitchLMChatBot.Models;

namespace TwitchLMChatBot.Application.Abstractions
{
    public interface IAccountRepository : IRepository<Account>
    {
        Account FindByType(AccountType broadcaster);
        Account GetCurrent();
    }
}