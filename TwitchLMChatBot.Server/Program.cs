using LanguageExt.Pipes;
using Microsoft.Extensions.FileProviders;
using Serilog;
using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using TwitchLMChatBot.Application.Configuration;
using TwitchLMChatBot.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog(); // Use Serilog for logging

// Clear default logging providers
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);

// Add services to the container.
builder.Services.AddCors(corsOptions =>
{
    corsOptions.AddPolicy("Development", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Vite
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
} );
builder.Services.AddMvc();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAplicationLayer();
builder.Services.AddInfrastructureLayer(builder.Configuration);

// For detailed guidance on error handling and best practices, refer to:
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-8.0#problem-details
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("Development");
}

app.UseExceptionHandler();
app.UseStatusCodePages();

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();