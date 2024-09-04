using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using TwitchLib.Api.Interfaces;
using TwitchLMChatBot.Application.Abstractions;
using TwitchLMChatBot.Models;

namespace TwitchLMChatBot.Server.Controllers
{
    public class TwitchAuthController : Controller
    {
        private readonly ITwitchApp _twitchApp;
        private readonly ITwitchAPI _twitchAPI;
        private readonly IAccountRepository accountRepository;

        public TwitchAuthController(ITwitchApp twitchApp, ITwitchAPI twitchAPI, IAccountRepository accountRepository)
        {
            _twitchApp = twitchApp;
            _twitchAPI = twitchAPI;
            this.accountRepository = accountRepository;
        }

        [HttpGet("auth/twitch")]
        public IActionResult Index(AccountType accountType = AccountType.Broadcaster)
        {
            var state = JsonConvert.SerializeObject(new State
            {
                Referer = Request.Headers["Referer"].ToString(),
                AccountType = accountType,
            });
            return Redirect(_twitchApp.GetAuthorizationCodeUrl(state));
        }

        [HttpGet("auth/twitch/callback")]
        public async Task<IActionResult> TwitchCallback(string code, string state )
        {
            _twitchAPI.Settings.ClientId = _twitchApp?.ClientId;
            _twitchAPI.Settings.Secret = _twitchApp?.ClientSecret;

            var stateData = JsonConvert.DeserializeObject<State>(state);
            var authResponse = await _twitchApp.GetAccessTokenFromCode(code);
          
            _twitchAPI.Settings.AccessToken = authResponse.AccessToken;
            
            var usersResponse = await _twitchAPI.Helix.Users.GetUsersAsync();
            var user = usersResponse.Users.FirstOrDefault();
            var account = new Account()
            {
                Type = stateData.AccountType,
                Auth = new AccountAuth
                {
                    AccessToken = authResponse.AccessToken,
                    ExpiresIn = authResponse.ExpiresIn,
                    RefreshToken = authResponse.RefreshToken,
                    Scopes = authResponse.Scopes,
                    TokenType = authResponse.TokenType
                },
                User = new AccountUser()
                {
                    Id = user.Id,
                    BroadcasterType = user.BroadcasterType,
                    CreatedAt = user.CreatedAt,
                    Description = user.Description,
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Login = user.Login,
                    OfflineImageUrl = user.OfflineImageUrl,
                    ProfileImageUrl = user.ProfileImageUrl,
                    Type = user.Type,
                    ViewCount = user.ViewCount,
                }
            };
            accountRepository.Insert(account);

            if (!string.IsNullOrEmpty(stateData?.Referer))
            {
                return Redirect(stateData?.Referer);
            }

            return Ok(account);  // Redirige a tu frontend
        }
    }

    internal class State
    {
        public string Referer { get; set; }
        public AccountType AccountType { get; set; }
    }
}
