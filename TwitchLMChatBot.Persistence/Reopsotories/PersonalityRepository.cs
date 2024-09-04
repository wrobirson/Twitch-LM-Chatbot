using LiteDB;
using TwitchLMChatBot.Application.Abstractions;
using TwitchLMChatBot.Models;

namespace TwitchLMChatBot.Persistence.Reopsotories
{
    public class PersonalityRepository : LiteDbRepositoryBase<Personality>, IPersonalityRepository
    {

        public PersonalityRepository(ILiteDatabase db) : base(db, DbCollections.Personalities)
        {

        }

        public void Delete(Personality personality)
        {
            GetCollection().Delete(personality.Id);
        }

        public Personality GetDefault()
        {
            return GetCollection()
                .Include(a => a.Provider)
                .FindAll()
                .Where(a => a.IsDefault)
                .FirstOrDefault();
        }
    }
}
