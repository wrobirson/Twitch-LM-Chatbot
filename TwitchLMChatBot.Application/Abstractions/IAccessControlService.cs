using TwitchLMChatBot.Application.Services;
using TwitchLMChatBot.Models;

namespace TwitchLMChatBot.Application.Abstractions
{
    public interface IAccessControlService
    {
        bool Check(CheckAccessRequest checkRequest);
        AccessControl Get();
        void Set(SetAccessControlRequest request);
    }

    public interface IMessageRecivedRespository : IRepository<MessageRecived>
    {
        IEnumerable<MessageRecived> FindByUserName(string userName, int count = 5);

        IEnumerable<MessageRecived> FindByDate(DateTime date, int count= 100);
    }
}