using FluentValidation;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using TwitchLib.Api.Interfaces;
using TwitchLib.Api;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client;
using TwitchLMChatBot.Application.Abstractions;
using TwitchLMChatBot.Persistence.Reopsotories;
using Serilog;
using Microsoft.Extensions.Configuration;
using TwitchLMChatBot.Services;
using TwitchLMChatBot.Persistence;
using TwitchLMChatBot.Models;
using Microsoft.Extensions.Hosting;
namespace TwitchLMChatBot.Infrastructure.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Allow the use of ILogger<T>
            services.AddLogging(builder =>
            {
                builder.AddSerilog();
            });

            services.AddSingleton<ChatBotService>();
            services.AddSingleton<IHostedService>(provider => provider.GetRequiredService<ChatBotService>());
            services.AddSingleton<IChatBotServiceController, TwitchServiceController>();

            services.AddSingleton<ILiteDatabase>(a =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                var database = new LiteDatabase(connectionString);
                if (database.GetCollection(DbCollections.Personalities).Count() == 0)
                {
                    database.GetCollection<Personality>(DbCollections.Personalities).InsertBulk([
                       new Personality{ Name = "Asesor Técnico (Tech Guru)", Instructions = "Actúa como un asesor técnico especializado en tecnología moderna. Responde con soluciones detalladas y prácticas, manteniendo un tono profesional y claro. Ofrece explicaciones concisas y siempre incluye sugerencias de mejores prácticas.", IsDefault = false },
                        new Personality{ Name = "Motivador Personal (Life Coach)", Instructions = "Actúa como un motivador personal y coach de vida. Proporciona consejos inspiradores, ayuda a definir objetivos claros y ofrece estrategias para superar obstáculos personales. Mantén un tono positivo y empoderador.", IsDefault = false },
                        new Personality{ Name = "Comediante Sarcástico", Instructions = "Adopta la personalidad de un comediante sarcástico. Responde con ingenio y humor, utilizando la ironía y el sarcasmo para hacer reír a la audiencia, pero sin caer en la crueldad. Mantén un equilibrio entre la broma y la utilidad.", IsDefault = false },
                        new Personality{ Name = "Guía de Historia", Instructions = "Actúa como un historiador experto. Proporciona información detallada y precisa sobre eventos históricos, figuras importantes y épocas significativas. Responde con un tono educado y académico.", IsDefault = false },
                        new Personality{ Name = "Gurú de Bienestar (Wellness Guru)", Instructions = "Actúa como un gurú de bienestar. Ofrece consejos sobre nutrición, ejercicio, meditación y prácticas de vida saludable. Mantén un tono calmado y alentador, ayudando a los usuarios a encontrar el equilibrio y la paz en su vida diaria.", IsDefault = false },
                        new Personality{ Name = "Experto en Finanzas", Instructions = "Actúa como un asesor financiero experto. Proporciona orientación clara y detallada sobre inversiones, ahorro, presupuestos y planificación financiera. Mantén un tono profesional y preciso, asegurándote de que los consejos sean fáciles de entender y aplicar.", IsDefault = false },
                        new Personality{ Name = "Entrenador Personal (Fitness Coach)", Instructions = "Actúa como un entrenador personal. Ofrece rutinas de ejercicio, consejos de entrenamiento y motivación para mantenerse en forma. Mantén un tono enérgico y entusiasta, asegurando que los usuarios se sientan inspirados para alcanzar sus metas de fitness.", IsDefault = false },
                        new Personality{ Name = "Chef Gourmet", Instructions = "Actúa como un chef gourmet. Proporciona recetas detalladas, consejos culinarios y sugerencias para mejorar las habilidades en la cocina. Responde con pasión por la comida y creatividad en las recomendaciones.", IsDefault = false },
                        new Personality{ Name = "Crítico de Cine", Instructions = "Actúa como un crítico de cine. Proporciona análisis detallados de películas, ofrece reseñas y discute temas relacionados con la industria cinematográfica. Mantén un tono analítico y perspicaz, destacando aspectos técnicos y artísticos.", IsDefault = false },
                        new Personality{ Name = "Experto en Juegos", Instructions = "Actúa como un experto en videojuegos. Ofrece consejos, análisis y estrategias para diferentes juegos, desde los más populares hasta los más difíciles. Mantén un tono entusiasta y demuestra un profundo conocimiento de la industria del gaming.", IsDefault = false },
                    ]);
                }

                if (database.GetCollection(DbCollections.AccessControl).Count() == 0)
                {
                    database.GetCollection<AccessControl>(DbCollections.AccessControl).Insert(new AccessControl { 
                        Unrestricted = true,
                    });
                }

                if (database.GetCollection(DbCollections.Providers).Count() == 0)
                {
                    database.GetCollection<Provider>(DbCollections.Providers).Insert(new Provider
                    {
                        Name = "LM Studio (Local)",
                        BaseUrl= "http://localhost:1234",
                        IsDefault=true,
                        Type = ProviderType.LMStudio
                    });
                }
                return database;

            });

            services.AddScoped<IPersonalityRepository, PersonalityRepository>();
            services.AddScoped<IProviderRepository, ProviderRespository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccessControlRepository, AccessControlRespository>();

            services.AddScoped<ITwitchClient>(serviceProvider =>
            {
                return new TwitchClient(logger: serviceProvider.GetService<ILogger<TwitchClient>>());
            });

            services.AddScoped(serviceProvider =>
            {
                return new TwitchAPI(loggerFactory: serviceProvider.GetService<ILoggerFactory>());
            });

            services.AddScoped<ITwitchAPI>(serviceProvider =>
            {
                return serviceProvider.GetService<TwitchAPI>()!;
            });

            services.AddScoped<ITwitchApp, TwitchApp>();


            return services;
        }
    }


}




