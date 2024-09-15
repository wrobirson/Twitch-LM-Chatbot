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

        public IEnumerable<MessageRecived> FindByDate(DateTime date, int count = Int32.MaxValue)
        {
            return GetCollection().Query()
                .Where(a => a.DateTime >= date.Date)
                .OrderByDescending(a => a.DateTime)
                .Limit(count)
                .ToList();
        }

        public IEnumerable<MessageRecived> FindByUserName(string userName, int count = 5)
        {
           return GetCollection().Query()
                .Where(a=> a.UserName.ToLower() == userName.ToLower())
                .OrderByDescending(a=> a.DateTime)
                .Limit(count)
                .ToList();
        }

    }
}
