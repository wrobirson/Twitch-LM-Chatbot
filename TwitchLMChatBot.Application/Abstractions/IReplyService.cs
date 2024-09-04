using OpenAI.Chat;
using TwitchLMChatBot.Models;

namespace TwitchLMChatBot.Application.Abstractions
{
    public interface IReplyService
    {
        string GetResponse(string message);
    }
}