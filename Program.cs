
using Lesson26;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Polling;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddSingleton<IUpdateHandler, BotUpdateHandler>();

builder.Services.AddSingleton<ITelegramBotClient, TelegramBotClient>( x =>
    new TelegramBotClient(builder.Configuration["Bot:Token"]
         ?? throw new ArgumentException("Telegram bot token is not configured")));

builder.Services.AddHostedService<BotHostedService>();

var app = builder.Build();

app.Run();