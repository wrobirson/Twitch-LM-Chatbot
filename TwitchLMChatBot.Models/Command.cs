using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchLMChatBot.Models
{
    public class Command : Entity
    {
        public string Name { get; set; }

        public bool UsingAI { get; set; }

        public string Response { get; set; }

        public bool IsEnabled { get; set; }

    }
}
