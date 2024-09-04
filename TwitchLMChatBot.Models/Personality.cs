using LiteDB;

namespace TwitchLMChatBot.Models
{
    public class Personality : Entity
    {
        public string Name { get; set; }

        public string Model { get; set; }

        public string Instructions { get; set; }

        public bool IsDefault { get; set; }

        public Provider Provider { get; set; }

        public string PrividerName
        {
            get
            {
                return Provider?.Name;
            }
        }
    }


}
