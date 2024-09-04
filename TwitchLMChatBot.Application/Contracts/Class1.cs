using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchLMChatBot.Application.Contracts
{
    public record CreatePersonalityRequest(string PersonalityName, string Instructions);

    public record UpdatePersonalityRequest(string PersonalityName, string Instructions);
}
