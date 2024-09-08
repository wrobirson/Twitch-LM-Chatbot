using LiteDB;
using TwitchLMChatBot.Application.Abstractions;
using TwitchLMChatBot.Models;

namespace TwitchLMChatBot.Persistence.Reopsotories
{
    public class CommandRepository : LiteDbRepositoryBase<Command>, ICommandRespository
    {
        public CommandRepository(ILiteDatabase db) : base(db, DbCollections.Commands)
        {

        }
    }
}
