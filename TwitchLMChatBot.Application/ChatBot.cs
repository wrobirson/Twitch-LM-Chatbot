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
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using TwitchLMChatBot.Application.Abstractions;
using TwitchLMChatBot.Models;

namespace TwitchLMChatBot.Application
{
    internal class ChatBot : IChatBot
    {
        private readonly IConfiguration configuration;
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
            ICommandRespository commandRespository)
        {

            _logger = logger;
            _accountRepository = accountRepository;
            _providerRepository = providerRepository;
            _personalityRepository = personalityRepository;
            this.configuration = configuration;
            _twitchClient = twitchClient;
            _twitchAPI = twitchAPI;
            _twitchApp = twitchApp;
            _replyService = chatClient;
            this._accessControlService = accessControlService;
            this._commandRespository = commandRespository;
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
        private async void OnMessageReceived(object? sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            var joinedChannel = _twitchClient.JoinedChannels.First();
            var (command, message) = ExtractCommandAndMessage(e.ChatMessage.Message);
            var commands = _commandRespository.FindAll();

            var commandWithoutSymbol = command.Trim('!');
            var commandFound = commands.FirstOrDefault(a => a.Name == commandWithoutSymbol);
            if (commandFound == null) {
               _logger.LogInformation($"{command} ignored. No command regisry found.");
               return;
            }

            if (commandFound.UsingAI)
            {
                var personaltity = _personalityRepository.GetDefault();
                var provider = _providerRepository.GetDefault();
                var prompt = commandFound.Response
                   .Replace("{user}", e.ChatMessage.Username)
                   .Replace("{input}", message);
                var response = await GetChatResponse(provider, personaltity, prompt);
                var chunks = SplitTextIntoChunks(response);
                _twitchClient.SendReply(_twitchClient.JoinedChannels.First(), e.ChatMessage.Id,
                  response);
            }
            else
            {
                var response = commandFound.Response
                    .Replace("{user}", e.ChatMessage.Username)
                    .Replace("{input}", message);
                _twitchClient.SendReply(_twitchClient.JoinedChannels.First(), e.ChatMessage.Id,
                    response);
            }


            if (command == ("!ia")  )
            {
                var haveAccess = e.ChatMessage.IsBroadcaster || _accessControlService.Check(new CheckAccessRequest
                {
                    IsFollower = await IsUserFollower(e.ChatMessage.UserId, joinedChannel.Channel),
                    IsModerator = e.ChatMessage.IsModerator,
                    IsSubscriber = e.ChatMessage.IsSubscriber,
                    IsVip = e.ChatMessage.IsVip
                });

                if (!haveAccess)
                {
                    _twitchClient.SendReply(joinedChannel, e.ChatMessage.Id, "You do not have access.");
                    return;
                }

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

                try
                {
                    var response = await GetChatResponse(provider, personaltity, $"El usario {e.ChatMessage.Username} dice: {message}");
                    var chunks = SplitTextIntoChunks(response);

                    foreach (var item in chunks)
                    {
                        _twitchClient.SendReply(_twitchClient.JoinedChannels.First(), e.ChatMessage.Id, item);
                        //_twitchClient.SendMessage(_twitchClient.JoinedChannels.First(),  item);
                    }
                }
                catch (Exception ex)
                {
                    _twitchClient.SendReply(_twitchClient.JoinedChannels.First(), e.ChatMessage.Id, ex.Message);
                }

            }

        }

        private async Task<bool> IsUserFollower(string userId, string channel)
        {
            var followers = await _twitchAPI.Helix.Users.GetUsersFollowsAsync(toId: channel, fromId: userId);
            return followers.TotalFollows > 0;
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
            var palabras = texto.Split(' ', '\n', '\t', '\r');
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

        private void OnConnected(object? sender, TwitchLib.Client.Events.OnConnectedArgs e)
        {
            _twitchClient.SendMessage(_twitchClient.JoinedChannels.First(), "VoHiYo Twitch LM Chat Online ");
        }

        private async Task<string> GetChatResponse(Provider provider, Personality personality, string prompt)
        {
            return _replyService.GetResponse(prompt);
        }
    }



}
