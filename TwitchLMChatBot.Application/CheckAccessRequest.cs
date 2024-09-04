
namespace TwitchLMChatBot.Application
{
    public class CheckAccessRequest
    {
        public bool IsFollower { get;  set; }
        public bool IsModerator { get;  set; }
        public bool IsSubscriber { get;  set; }
        public bool IsVip { get;  set; }
    }
}