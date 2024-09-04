using Serilog.Events;

namespace TwitchLMChatBot.Application.Abstractions
{
    public interface ILogEventEmiter
    {
        event EventHandler<LogEvent> EventEmited;
    }
}