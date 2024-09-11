using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Net;
using TwitchLib.Api;
using TwitchLib.Api.Auth;
using TwitchLib.Api.Core.Enums;
using TwitchLib.Api.Helix.Models.Channels.GetChannelEditors;
using TwitchLib.Api.Helix.Models.Users.GetUsers;
using TwitchLMChatBot.Application.Abstractions;

namespace TwitchLMChatBot.Services
{
    /// <summary>
    /// La clase <c>TwitchApp</c> proporciona una implementación de la interfaz <c>ITwitchApp</c>
    /// para interactuar con la API de Twitch. Esta clase maneja la autenticación OAuth2 
    /// y la obtención de información del usuario utilizando el SDK de Twitch.
    /// </summary>
    public class TwitchApp : ITwitchApp
    {

        private readonly TwitchAPI _twitchAPI;

        public TwitchApp(TwitchAPI twitchAPI, IConfiguration configuration)
        {
            _twitchAPI = twitchAPI;
            ClientId = configuration["Twitch:ClientId"]!;
            ClientSecret = configuration["Twitch:ClientSecret"]!;
            RedirectUri = configuration["Twitch:RedirectUri"]!;
        }

        public string ClientId { get; }
        public string ClientSecret { get; }
        public string RedirectUri { get; }

        public async Task<GetUsersResponse> GetUserInfoAsync(string accessToken)
        {
            return await _twitchAPI.Helix.Users.GetUsersAsync(accessToken: accessToken);
        }

        public async Task<AuthCodeResponse> AuthorizeAsync()
        {
            // Genera la URL de autenticación con los scopes necesarios
            string authorizationUrl = GetAuthorizationCodeUrl();

            // Inicia HttpListener para escuchar la redirección con el código de autorización
            using var listener = new HttpListener();
            listener.Prefixes.Add($"{RedirectUri}/");
            listener.Start();

            // Abre el navegador para autenticarse
            Process.Start(new ProcessStartInfo() { UseShellExecute = true, FileName = authorizationUrl });

            // Espera la redirección con el código de autorización
            var context = await listener.GetContextAsync();
            var query = context.Request.Url.Query;
            var authorizationCode = ExtractCodeFromQuery(query);

            // Envía una respuesta al navegador
            var response = context.Response;
            var responseString = @"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Authentication Successful</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            color: #333;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            margin: 0;
        }
        .container {
            text-align: center;
            background-color: #fff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        }
        h1 {
            font-size: 24px;
            margin-bottom: 10px;
            color: #4CAF50;
        }
        p {
            font-size: 16px;
            margin-bottom: 20px;
        }
        .close-button {
            display: inline-block;
            padding: 10px 20px;
            background-color: #4CAF50;
            color: #fff;
            text-decoration: none;
            border-radius: 5px;
            transition: background-color 0.3s;
        }
        .close-button:hover {
            background-color: #45a049;
        }
    </style>
</head>
<body>
    <div class=""container"">
        <h1>Authentication Successful</h1>
        <p>You can close this window.</p>
        <a href=""javascript:window.close();"" class=""close-button"">Close Window</a>
    </div>
</body>
</html>
";
            var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            var responseOutput = response.OutputStream;
            await responseOutput.WriteAsync(buffer, 0, buffer.Length);
            responseOutput.Close();

            // Intercambia el código por un token de acceso
            var authResponse = await GetAccessTokenFromCode(authorizationCode);

            listener.Stop();

            _twitchAPI.Settings.ClientId = ClientId;
            _twitchAPI.Settings.AccessToken = authResponse.AccessToken;

            return authResponse;

        }

        public async Task<AuthCodeResponse> GetAccessTokenFromCode(string authorizationCode)
        {
            _twitchAPI.Settings.ClientId = ClientId;
            _twitchAPI.Settings.Secret = ClientSecret;
            return await _twitchAPI.Auth
                            .GetAccessTokenFromCodeAsync(authorizationCode, ClientSecret, RedirectUri, ClientId);
        }

        public string GetAuthorizationCodeUrl(string state = null)
        {
            _twitchAPI.Settings.ClientId = ClientId;
            _twitchAPI.Settings.Secret = ClientSecret;
            return _twitchAPI.Auth.GetAuthorizationCodeUrl(RedirectUri, [

                      AuthScopes.Channel_Check_Subscription,
                      AuthScopes.Channel_Commercial,
                      AuthScopes.Channel_Editor,
                      AuthScopes.Channel_Feed_Edit,
                      AuthScopes.Channel_Feed_Read,
                      AuthScopes.Channel_Read,
                      AuthScopes.Channel_Stream,
                      AuthScopes.Channel_Subscriptions,
                      AuthScopes.Chat_Read,
                      AuthScopes.Chat_Edit,
                      AuthScopes.Collections_Edit,
                      AuthScopes.Communities_Edit,
                      AuthScopes.Communities_Moderate,
                      AuthScopes.User_Blocks_Edit,
                      AuthScopes.User_Blocks_Read,
                      AuthScopes.User_Follows_Edit,
                      AuthScopes.User_Read,
                      AuthScopes.User_Subscriptions,
                      AuthScopes.Viewing_Activity_Read,
                      AuthScopes.OpenId,
                      AuthScopes.Helix_Analytics_Read_Extensions,
                      AuthScopes.Helix_Analytics_Read_Games,
                      AuthScopes.Helix_Bits_Read,
                      AuthScopes.Helix_Channel_Edit_Commercial,
                      AuthScopes.Helix_Channel_Manage_Broadcast,
                      AuthScopes.Helix_Channel_Manage_Extensions,
                      AuthScopes.Helix_Channel_Manage_Moderators,
                      AuthScopes.Helix_Channel_Manage_Polls,
                      AuthScopes.Helix_Channel_Manage_Predictions,
                      AuthScopes.Helix_Channel_Manage_Redemptions,
                      AuthScopes.Helix_Channel_Manage_Schedule,
                      AuthScopes.Helix_Channel_Manage_Videos,
                      AuthScopes.Helix_Channel_Manage_VIPs,
                      AuthScopes.Helix_Channel_Read_Charity,
                      AuthScopes.Helix_Channel_Read_Editors,
                      AuthScopes.Helix_Channel_Read_Goals,
                      AuthScopes.Helix_Channel_Read_Hype_Train,
                      AuthScopes.Helix_Channel_Read_Polls,
                      AuthScopes.Helix_Channel_Read_Predictions,
                      AuthScopes.Helix_Channel_Read_Redemptions,
                      AuthScopes.Helix_Channel_Read_Stream_Key,
                      AuthScopes.Helix_Channel_Read_Subscriptions,
                      AuthScopes.Helix_Channel_Read_VIPs,
                      AuthScopes.Helix_Clips_Edit,
                      AuthScopes.Helix_Moderation_Read,
                      AuthScopes.Helix_Moderator_Manage_Banned_Users,
                      AuthScopes.Helix_Moderator_Manage_Blocked_Terms,
                      AuthScopes.Helix_Moderator_Manage_Announcements,
                      AuthScopes.Helix_Moderator_Manage_Automod,
                      AuthScopes.Helix_Moderator_Manage_Automod_Settings,
                      AuthScopes.Helix_moderator_Manage_Chat_Messages,
                      AuthScopes.Helix_Moderator_Manage_Chat_Settings,
                      AuthScopes.Helix_Moderator_Read_Blocked_Terms,
                      AuthScopes.Helix_Moderator_Read_Automod_Settings,
                      AuthScopes.Helix_Moderator_Read_Chat_Settings,
                      AuthScopes.Helix_Moderator_Read_Chatters,
                      AuthScopes.Helix_User_Edit,
                      AuthScopes.Helix_User_Edit_Broadcast,
                      AuthScopes.Helix_User_Edit_Follows,
                      AuthScopes.Helix_User_Manage_BlockedUsers,
                      AuthScopes.Helix_User_Manage_Chat_Color,
                      AuthScopes.Helix_User_Manage_Whispers,
                      AuthScopes.Helix_User_Read_BlockedUsers,
                      AuthScopes.Helix_User_Read_Broadcast,
                      AuthScopes.Helix_User_Read_Email,
                      AuthScopes.Helix_User_Read_Follows,
                      AuthScopes.Helix_User_Read_Subscriptions,
                      AuthScopes.Helix_Moderator_Read_Followers
                ],
                    clientId: ClientId,
                    state: state);
        }

        private string ExtractCodeFromQuery(string query)
        {
            var queryParams = System.Web.HttpUtility.ParseQueryString(query);
            return queryParams.Get("code");
        }

    }
}
