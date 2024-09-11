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

        public CommandPermissions Permissions { get; set; }
    }


    public class CommandPermissions
    {
        public bool Viewers { get; set; }
        public bool Followers { get; set; }
        public bool Subscribers { get; set; }
        public bool Vips { get; set; }
        public bool Moderators { get; set; }
    }
}
