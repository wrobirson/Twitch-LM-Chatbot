using OpenAI.Chat;
using TwitchLMChatBot.Models;

namespace TwitchLMChatBot.Application.Abstractions
{
    public interface IReplyService
    {
        string GetResponse(string message);
    }

    public interface IDateTimeService
    {
        DateTime Now { get; }
    }


    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
    }
}