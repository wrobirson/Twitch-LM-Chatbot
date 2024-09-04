namespace TwitchLMChatBot.Infrastructure
{
    public class TwitchServiceController : IChatBotServiceController
    {
        private readonly ChatBotService _twitchService;

        public TwitchServiceController(ChatBotService twitchService)
        {
            _twitchService = twitchService;
        }

        public Task RestartAsync()
        {
            return _twitchService.RestartAsync();
        }
    }
}
