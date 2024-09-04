using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLMChatBot.Application.Abstractions;
using TwitchLMChatBot.Models;

namespace TwitchLMChatBot.Persistence.Reopsotories
{
    public class AccountRepository : LiteDbRepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(ILiteDatabase db) : base(db, DbCollections.Accounts)
        {

        }

        public Account FindByType(AccountType accountType)
        {
            return GetCollection().Query().Where(a=> a.Type == accountType).FirstOrDefault();
        }

        public Account GetCurrent()
        {
            return GetCollection().Query().FirstOrDefault();
        }
    }
}
