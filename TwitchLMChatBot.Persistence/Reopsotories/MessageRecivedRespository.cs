using LiteDB;
using TwitchLMChatBot.Application.Abstractions;
using TwitchLMChatBot.Models;

namespace TwitchLMChatBot.Persistence.Reopsotories
{
    public class MessageRecivedRespository : LiteDbRepositoryBase<MessageRecived>, IMessageRecivedRespository
    {
        public MessageRecivedRespository(ILiteDatabase db) : base(db, DbCollections.MessagesReceived)
        {
        }

        public IEnumerable<MessageRecived> FindByUserName(string userName)
        {
           return GetCollection().Query().Where(a=> a.UserName.ToLower() == userName.ToLower()).ToList();
        }
    }
}
