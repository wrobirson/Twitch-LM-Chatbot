
using LiteDB;
using System.Text.Json.Serialization;

namespace TwitchLMChatBot.Models
{
    public class Provider : Entity
    {
        public string Name { get; set; }

        public ProviderType Type { get; set; }

        public string BaseUrl { get; set; }

        public string ApiKey { get; set; }

        public bool IsDefault { get; set; }    

        [BsonIgnore]
        public string TypeName => Type switch
        {
            ProviderType.LMStudio => "LM Studio",
            ProviderType.OpenAi => "Open AI",
            _ => "Not defined.",
        };

    }

}
