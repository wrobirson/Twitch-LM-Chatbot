using TwitchLib.Client.Models;

namespace TwitchLMChatBot.Application
{
    public interface IMessageVariablesService
    {
        string ReplaceVariables(string response, ChatMessage chatMessage);
    }
}