using Serilog.Core;
using Serilog.Events;
using TwitchLMChatBot.Application.Abstractions;

namespace TwitchLMChatBot.Infrastructure.Logging
{
    public class LogEventEmiter : ILogEventSink, ILogEventEmiter
    {
        public event EventHandler<LogEvent> EventEmited;

        public void Emit(LogEvent logEvent)
        {
            EventEmited?.Invoke(this, logEvent);
        }
    }
}




