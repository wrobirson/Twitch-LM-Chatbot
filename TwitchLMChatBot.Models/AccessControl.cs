namespace TwitchLMChatBot.Models
{
    public class AccessControl : Entity
    {
        public bool Unrestricted { get; set; }
        public bool Followers { get; set; }
        public bool Subscribers { get; set; }
        public bool Moderators { get; set; }
        public bool Vips { get; set; }
    }

}
