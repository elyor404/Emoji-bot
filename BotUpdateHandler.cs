using Lesson26.BotMessage;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
class BotUpdateHandler(
    IBotMessage botSendMassage,
    ILogger<BotUpdateHandler> logger) : IUpdateHandler
{
    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Something happened wrong - {messaga}", exception.Message);
        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        await botSendMassage.CommunicateAsync(botClient, update, cancellationToken);
    }
}