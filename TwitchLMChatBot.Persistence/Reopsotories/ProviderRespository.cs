using LiteDB;
using TwitchLMChatBot.Application.Abstractions;
using TwitchLMChatBot.Models;

namespace TwitchLMChatBot.Persistence.Reopsotories
{
    public class ProviderRespository : LiteDbRepositoryBase<Provider>, IProviderRepository
    {
        public ProviderRespository(ILiteDatabase db) : base(db, DbCollections.Providers)
        {

        }

        public Provider GetByType(ProviderType type)
        {
            return GetCollection().Query()
                .Where(a => a.Type == ProviderType.LMStudio)
                .FirstOrDefault();
        }

        public Provider GetDefault()
        {
            return GetCollection().Query()
                .Where(a => a.IsDefault == true)
                .Single();
        }
    }
}
