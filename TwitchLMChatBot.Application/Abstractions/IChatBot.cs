namespace TwitchLMChatBot.Application.Abstractions
{
   public  interface IChatBot
    {
        Task<bool> Connect();
        void Disconnect();
    }
}