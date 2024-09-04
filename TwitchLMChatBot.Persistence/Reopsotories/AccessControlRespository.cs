using LiteDB;
using TwitchLMChatBot.Application.Abstractions;
using TwitchLMChatBot.Models;

namespace TwitchLMChatBot.Persistence.Reopsotories
{
    public class AccessControlRespository : LiteDbRepositoryBase<AccessControl>, IAccessControlRepository
    {
        public AccessControlRespository(ILiteDatabase db) : base(db, DbCollections.AccessControl)
        {
        }
    }
}
