namespace TwitchLMChatBot.Application.Exceptions
{
    [Serializable]
    public class ProviderNotFoundException : Exception
    {
        public ProviderNotFoundException()
        {
        }

        public ProviderNotFoundException(string? message) : base(message)
        {
        }

        public ProviderNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public ProviderNotFoundException(int providerId)
        {
            ProviderId = providerId;
        }

        public int ProviderId { get; }
    }
}
