namespace TwitchLMChatBot.Application.Exceptions
{
    [Serializable]
    public class PersonalityNotFoundException : Exception
    {
        public int PersonalityId { get; }

        public PersonalityNotFoundException(int personalityId) : base($"Personality with ID {personalityId} not found.")
        {
            PersonalityId = personalityId;
        }

        public PersonalityNotFoundException()
        {
        }

        public PersonalityNotFoundException(string? message) : base(message)
        {
        }

        public PersonalityNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

    }
}