using TwitchLib.Client.Models;
using TwitchLMChatBot.Application.Abstractions;

namespace TwitchLMChatBot.Application
{
    public class MessageVariablesService : IMessageVariablesService
    {
        private readonly ICommandRespository _commandRespository;
        private readonly IMessageRecivedRespository _messageRecivedRespository;
        private readonly IDateTimeService _dateTimeService;
        private Dictionary<string, Func<ChatMessage, string>> _messageVariables;

        public MessageVariablesService(ICommandRespository commandRespository, IMessageRecivedRespository messageRecivedRespository,
            IDateTimeService dateTimeService)
        {
            _commandRespository = commandRespository;
            _messageRecivedRespository = messageRecivedRespository;
            _dateTimeService = dateTimeService;
            _messageVariables = new Dictionary<string, Func<ChatMessage, string>>()
                {
                    { "{commands}" , RenderCommandList },
                    { "{userName}", RenderUserName},
                    { "{commandInput}", RenderCommandInput},
                    { "{userMessages}", RenderUserMessages},
                    { "{allUsers}", RenderAllUsers },
                    { "{allMessages}", RenderAllMessages },
                    { "{allUsersWithMessages}", RenderAllUsersWithMessages }
                };
        }

        public string ReplaceVariables(string textWithVariables, ChatMessage chatMessage)
        {
            var resultText = textWithVariables;
            foreach (var item in _messageVariables)
            {
                resultText = resultText.Replace(item.Key, item.Value.Invoke(chatMessage));
            }
            return resultText;
        }

        private string RenderAllUsersWithMessages(ChatMessage message)
        {
            var date = _dateTimeService.Now;
            var messages = _messageRecivedRespository.FindByDate(date);
            if (messages.Count() > 0)
            {
                string joinedMessages = string.Join("\n", messages.Select(a => $"{a.UserName}:{a.Message}."));
                return joinedMessages;
            }
            else
            {
                return string.Format("No hay mesnajes registrados para extraer los mensajes de cada usuario.");
            }
        }

        private string RenderAllMessages(ChatMessage message)
        {
            var date = _dateTimeService.Now;
            var messages = _messageRecivedRespository.FindByDate(date);
            if (messages.Count() > 0)
            {
                var joinedMessages = string.Join(".", messages.Select(a => $"{a.Message}"));
                return joinedMessages;
            }
            else
            {
                return string.Format("No hay mesnajes registrados.");
            }
        }

        private string RenderAllUsers(ChatMessage message)
        {
            var date = _dateTimeService.Now;

            var usernames = _messageRecivedRespository.FindByDate(date)
               .Select(a => a.UserName)
               .Distinct();

            if (usernames.Count() > 0)
            {
                var joinedMessages = string.Join(", ", usernames);
                return joinedMessages;
            }
            else
            {
                return string.Format("No hay mensajes registrado para extraer los usarios.");
            }
        }

        private string RenderUserMessages(ChatMessage chatMessage)
        {
            var messages = _messageRecivedRespository.FindByUserName(chatMessage.Username);
            if (messages.Count() > 0)
            {
                var joinedMessages = string.Join(".", messages.Select(a => a.Message));
                return joinedMessages;
            }
            else
            {
                return string.Format("El usuario {0} no tiene mensajes registrados.",chatMessage.Username);
            }
        }

        private string RenderCommandInput(ChatMessage message)
        {
            return message.Message;
        }

        private string RenderUserName(ChatMessage message)
        {
            return message.Username;
        }

        private string RenderCommandList(ChatMessage message)
        {
            var commands = _commandRespository.FindAll();
            string commandListText = string.Join(" | ", commands.Select(a => "!" + a.Name));
            return commandListText;
        }

       
    }



}
