using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Api;
using TwitchLib.Api.Interfaces;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using TwitchLMChatBot.Application.Abstractions;
using TwitchLMChatBot.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TwitchLMChatBot.Application
{
    internal class ChatBot : IChatBot
    {
        private readonly IConfiguration _configuration;
        private readonly ITwitchClient _twitchClient;
        private readonly ILogger<ChatBot> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IProviderRepository _providerRepository;
        private readonly IPersonalityRepository _personalityRepository;
        private readonly TwitchAPI _twitchAPI;
        private readonly ITwitchApp _twitchApp;
        private readonly IReplyService _replyService;
        private readonly IAccessControlService _accessControlService;
        private readonly ICommandRespository _commandRespository;
        private readonly IMessageRecivedRespository _messageRecivedRespository;
        private readonly IDateTimeService _dateTimeService;

        public ChatBot(
            IConfiguration configuration,
            ITwitchClient twitchClient,
            ILogger<ChatBot> logger,
            IAccountRepository accountRepository,
            IProviderRepository providerRepository,
            IPersonalityRepository personalityRepository,
            TwitchAPI twitchAPI,
            ITwitchApp twitchApp,
            IReplyService chatClient,
            IAccessControlService accessControlService,
            ICommandRespository commandRespository,
            IMessageRecivedRespository messageRecivedRespository,
            IDateTimeService dateTimeService)
        {

            _logger = logger;
            _accountRepository = accountRepository;
            _providerRepository = providerRepository;
            _personalityRepository = personalityRepository;
            _configuration = configuration;
            _twitchClient = twitchClient;
            _twitchAPI = twitchAPI;
            _twitchApp = twitchApp;
            _replyService = chatClient;
            _accessControlService = accessControlService;
            _commandRespository = commandRespository;
            _messageRecivedRespository = messageRecivedRespository;
            _dateTimeService = dateTimeService;
        }

        public async Task<bool> Connect()
        {
            bool connected = false;
            var account = _accountRepository.GetCurrent();
            if (account != null)
            {
                var valiadationResponse = await _twitchAPI.Auth.ValidateAccessTokenAsync(account.Auth.AccessToken);
                if (valiadationResponse == null)
                {
                    var refreshResponse = await _twitchAPI.Auth.RefreshAuthTokenAsync(
                        account.Auth.RefreshToken,
                        _twitchApp.ClientSecret,
                        _twitchApp.ClientId);

                    account.Auth = new AccountAuth
                    {
                        AccessToken = refreshResponse.AccessToken,
                        RefreshToken = refreshResponse.RefreshToken,
                        ExpiresIn = refreshResponse.ExpiresIn,
                        Scopes = account.Auth.Scopes,
                        TokenType = account.Auth.TokenType
                    };
                    _accountRepository.Update(account);
                }

                _twitchClient.Initialize(new ConnectionCredentials(account.User.Login, account.Auth.AccessToken), account.User.Login);
                connected = _twitchClient.Connect();
                _twitchClient.OnConnected += OnConnected;
                _twitchClient.OnMessageReceived += OnMessageReceived;
            }
            else
            {
                _logger.LogInformation("Twitch is not set. Client initialization omited.");
            }
            return connected;
        }

        public async void Disconnect()
        {
            if (_twitchClient.IsInitialized && _twitchClient.IsConnected && _twitchClient.JoinedChannels.Any())
            {
                _twitchClient.SendMessage(_twitchClient.JoinedChannels.First(), "PoroSad Twitch LM Chat Off");
            }
        }

        private async Task OnMessageReceived(object? sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            try
            {

                SaveReceivedMessage(e);

                var account = _accountRepository.GetCurrent();
                var joinedChannel = new JoinedChannel(e.ChatMessage.Channel);
                var (command, message) = ExtractCommandAndMessage(e.ChatMessage.Message);
                var commands = _commandRespository.FindAll();

                var commandWithoutSymbol = command.Trim('!');
                var commandFound = commands.FirstOrDefault(a => a.Name == commandWithoutSymbol);
                if (commandFound == null)
                {
                    _logger.LogInformation($"{command} ignored. No command regisry found.");

                    //if (e.ChatMessage.Message.Contains(account.User.Login, StringComparison.OrdinalIgnoreCase))
                    //{
                    //    var personaltity = _personalityRepository.GetDefault();
                    //    var provider = _providerRepository.GetDefault();
                    //    var response = await GetChatResponse(provider, personaltity, e.ChatMessage.Message);
                    //    //var chunks = SplitTextIntoChunks(response);
                    //    _twitchClient.SendReply(joinedChannel, e.ChatMessage.Id,
                    //      response);
                    //}
                    return;
                }

                var accessMap = new[]{
                    (commandFound.Permissions.Viewers, true ),
                    (commandFound.Permissions.Followers, await IsUserFollower(e.ChatMessage.Username, joinedChannel.Channel)),
                    (commandFound.Permissions.Subscribers, e.ChatMessage.IsSubscriber),
                    (commandFound.Permissions.Vips, e.ChatMessage.IsVip),
                    (commandFound.Permissions.Moderators, e.ChatMessage.IsModerator),
                };

                bool canExecute = e.ChatMessage.IsBroadcaster || accessMap.Any(kvp => kvp.Item1 == kvp.Item2);

                if (!canExecute)
                {
                    _twitchClient.SendReply(joinedChannel, e.ChatMessage.Id, "You don have permission to run this command.");
                    _logger.LogInformation($"{e.ChatMessage.Username} can not execute command {commandFound.Name}");
                    return;
                }

                var macros = new Dictionary<string, Func<string>>()
                {
                    {"{commands}" , ()=>
                    {
                        var commands = _commandRespository.FindAll();
                        string commandListText = string.Join(" | ", commands.Select(a=> "!" + a.Name));
                        return commandListText;
                    } },
                    { "{userName}", ()=> {
                        return e.ChatMessage.Username;
                    }},

                    { "{commandInput}", ()=> {
                        return message;
                    }},

                    { "{userMessages}", ()=> {
                         var messages = _messageRecivedRespository.FindByUserName(e.ChatMessage.Username);
                         if (messages.Count() > 0)
                         {
                            var joinedMessages = string.Join(".", messages.Select(a => a.Message));
                            return joinedMessages;
                        }
                        else
                        {
                           return string.Format("El usuario {0} no tiene mensajes registrados.", e.ChatMessage.Username);
                        }
                    }},
                    { "{allUsers}", ()=>
                    {
                        var date = _dateTimeService.Now;

                         var usernames = _messageRecivedRespository.FindByDate(date)
                            .Select(a=> a.UserName)
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
                    } },
                    { "{allMessages}", ()=>
                    {
                        var date = _dateTimeService.Now;
                         var messages =  _messageRecivedRespository.FindByDate(date);
                         if (messages.Count() > 0)
                         {
                            var joinedMessages = string.Join(".", messages.Select(a => $"{a.Message}"));
                            return joinedMessages;
                        }
                        else
                        {
                           return string.Format("No hay mesnajes registrados.");
                        }
                    } },
                    { "{allUsersWithMessages}", ()=>
                    {
                        var date = _dateTimeService.Now;
                         var messages = _messageRecivedRespository.FindByDate(date);
                         if (messages.Count() > 0)
                         {
                            string joinedMessages =string.Join("\n", messages.Select(a => $"{a.UserName}:{a.Message}."));
                            return joinedMessages ;
                         }
                         else
                         {
                            return string.Format("No hay mesnajes registrados para extraer los mensajes de cada usuario.");
                         }
                    } }
                };

                // La respuesta ya construida luego de reempalazar las variables y obtener respuesta de la IA.
                string responseOutput = string.Empty;

                // Plantilla de la respuesta que se enviara al chat 
                string responseInput = commandFound.Response;

                foreach (var item in macros)
                {
                    responseInput = responseInput.Replace(item.Key, item.Value.Invoke());
                }

                if (commandFound.UsingAI)
                {
                    var personaltity = _personalityRepository.GetDefault();
                    var provider = _providerRepository.GetDefault();

                    if (personaltity == null)
                    {
                        _logger.LogInformation("Default personality not found.");
                        return;
                    }

                    if (provider == null)
                    {
                        _logger.LogInformation("Default provider not found.");
                        return;
                    }

                    responseOutput = await GetChatResponse(responseInput);
                }
                else
                {
                    responseOutput = responseInput;
                }

                var chunks = SplitTextIntoChunks(responseOutput);
                foreach (var chunksItem in chunks)
                {
                    _twitchClient.SendReply(joinedChannel, e.ChatMessage.Id, chunksItem);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }

        private void SaveReceivedMessage(OnMessageReceivedArgs e)
        {
            if (!e.ChatMessage.Message.StartsWith("!"))
            {
                _messageRecivedRespository.Insert(new MessageRecived
                {
                    UserName = e.ChatMessage.Username,
                    Message = e.ChatMessage.Message,
                    DateTime =_dateTimeService.Now,
                });
                _logger.LogInformation("Message saved.", e.ChatMessage);
            }
        }

        private async Task<bool> IsUserFollower(string userName, string channelName)
        {
            var account = _accountRepository.GetCurrent();
            _twitchAPI.Settings.ClientId = _twitchApp.ClientId;
            _twitchAPI.Settings.AccessToken = account.Auth.AccessToken;
            var user = await _twitchAPI.Helix.Users.GetUsersAsync(logins: new List<string> { userName });
            var channel = await _twitchAPI.Helix.Users.GetUsersAsync(logins: new List<string> { channelName });
            try
            {
                if (user.Users.Length > 0 && channel.Users.Length > 0)
                {
                    var userId = user.Users[0].Id;
                    string channelId = channel.Users[0].Id;
                    //var followers = await _twitchAPI.Helix.Users.GetUsersFollowsAsync(fromId: userId, toId: channelId);
                    var followers = await _twitchAPI.Helix.Channels.GetChannelFollowersAsync(broadcasterId: channelId, userId: userId);
                    return followers.Data.Length > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return false;
        }

        private (string Command, string Message) ExtractCommandAndMessage(string chatMessage)
        {
            if (string.IsNullOrWhiteSpace(chatMessage))
            {
                return (string.Empty, string.Empty);
            }

            // Separar el comando del mensaje asumiendo que el comando está precedido por '!'
            string[] parts = chatMessage.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

            // Verificar si el primer elemento es un comando
            if (parts.Length > 0 && parts[0].StartsWith("!"))
            {
                string command = parts[0];
                string message = parts.Length > 1 ? parts[1] : string.Empty;

                return (command, message);
            }

            // Si no hay comando, devolver cadenas vacías
            return (string.Empty, chatMessage);
        }

        private List<string> SplitTextIntoChunks(string texto, int longitudMaxima = 400)
        {
            var partes = new List<string>();
            var palabras = texto.Split([' ', '\n', '\t', '\r'], StringSplitOptions.RemoveEmptyEntries);
            var parteActual = "";

            foreach (var palabra in palabras)
            {
                // preveer la texto final
                var nuevaParte = parteActual + " " + palabra;
                if (nuevaParte.Length >= longitudMaxima)
                {
                    partes.Add(parteActual);
                    parteActual = palabra;
                }
                else
                {
                    parteActual = nuevaParte;
                }

            }

            if (!string.IsNullOrEmpty(parteActual))
            {
                partes.Add(parteActual);
            }

            return partes;
        }

        private async Task OnConnected(object? sender, TwitchLib.Client.Events.OnConnectedArgs e)
        {
            var account = _accountRepository.GetCurrent();

            _twitchClient.SendMessage(_twitchClient.JoinedChannels.First(), "VoHiYo Twitch LM Chat Online ");
        }

        private async Task<string> GetChatResponse(string prompt)
        {
            return _replyService.GetResponse(prompt);
        }
    }



}
