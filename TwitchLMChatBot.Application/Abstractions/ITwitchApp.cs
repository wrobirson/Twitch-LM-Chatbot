using TwitchLib.Api.Auth;
using TwitchLib.Api.Helix.Models.Users.GetUsers;

namespace TwitchLMChatBot.Application.Abstractions
{
    public interface ITwitchApp
    {
        string ClientId { get; }
        string ClientSecret { get; }

        Task<AuthCodeResponse> AuthorizeAsync();
        Task<AuthCodeResponse> GetAccessTokenFromCode(string authorizationCode);
        string GetAuthorizationCodeUrl(string state = null);
        Task<GetUsersResponse> GetUserInfoAsync(string accessToken);
    }
}