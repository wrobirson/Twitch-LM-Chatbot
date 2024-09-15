using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TwitchLMChatBot.Application.Abstractions;
using TwitchLMChatBot.Application.Services;

namespace TwitchLMChatBot.Application.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAplicationLayer(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.AddMediatR((a) =>
            {
                a.RegisterServicesFromAssembly(assembly);
            });

            services.AddValidatorsFromAssembly(assembly);

            services.AddScoped<IChatBot, ChatBot>();
            services.AddScoped<IReplyService, ReplayService>();
            services.AddScoped<IPersonalityService, PersonalityService>();
            services.AddScoped<IAccessControlService, AccessControlService>();
            services.AddScoped<IDateTimeService, DateTimeService>();

            return services;
        }
    }


}




