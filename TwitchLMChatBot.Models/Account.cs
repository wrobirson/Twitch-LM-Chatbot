namespace TwitchLMChatBot.Models
{
    public class Account : Entity
    {
        public AccountAuth Auth { get; set; }

        public AccountUser User { get; set; }

        public AccountType Type { get; set; }
    }

    public enum AccountType
    {
        Broadcaster,
        Bot
    }

    public class AccountUser
    {
        public string Id { get; set; }

        public string Login { get; set; }

        public string DisplayName { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Type { get; set; }

        public string BroadcasterType { get; set; }

        public string Description { get; set; }

        public string ProfileImageUrl { get; set; }

        public string OfflineImageUrl { get; set; }

        public long ViewCount { get; set; }

        public string Email { get; set; }
    }

    public class AccountAuth
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
        public string[] Scopes { get; set; }
        public string TokenType { get; set; }
    }

}
