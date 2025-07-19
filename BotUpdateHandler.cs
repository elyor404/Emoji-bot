using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
class BotUpdateHandler(ILogger<BotUpdateHandler> logger) : IUpdateHandler
{
    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Something happened wrong - {messaga}", exception.Message);
        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        logger.LogInformation("💌 New message from {client}", update.Message!.Chat.FirstName);

        if (update.Type is Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            logger.LogInformation("Request:{text}  - User: {firstname} - ID:{id}   ",
                update.Message!.Text,
                update.Message!.Chat.FirstName,
                update.Message!.Chat.Id);

            if (update.Message!.Text!.Trim().ToLower() == "/start")
            {
                await botClient.SendMessage(
                    chatId: update.Message.Chat.Id,
                    text: """
                        *If you need construction:*
                        - `/help`
                        """,
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                    cancellationToken: cancellationToken
                );
            }
            else if (update.Message!.Text!.Trim().ToLower() == "/help")
            {
                await botClient.SendMessage(
                    chatId: update.Message.Chat.Id,
                    text: """
                        *Supported requests:*

                        - `/fun-emoji seed`
                        - `/avataaars seed`
                        - `/bottts seed`
                        - `/pixel-art seed`

                        _E.g_: `/fun-emoji Elyor`
                        """,
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                    cancellationToken: cancellationToken
                );
            }
            else
            {
                List<string> patterns = ["/fun-emoji", "/avataaars", "/bottts", "/pixel-art"];
                var lst = update.Message!.Text?.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                string emoji = lst![0].ToLower() ?? "";
                string? seed = lst.Length >= 2 ? lst[1] : null; ;

                if (patterns.Contains(emoji) && seed is not null)
                {
                    var url = $"https://api.dicebear.com/8.x{emoji}/png?seed={seed}";
                    await botClient.SendPhoto(
                    chatId: update.Message.Chat.Id,
                    photo: url,
                    cancellationToken: cancellationToken);
                }
                else if (!patterns.Contains(emoji) && seed is not null)
                {
                    // "Noma’lum buyruq. Quyidagilardan birini ishlating: /fun-emoji, /bottts, /avataaars, /pixel-art"
                    await botClient.SendMessage(
                        chatId: update.Message.Chat.Id,
                        text: "Noma'lum buyruq. Quyidagilardan birini ishlating: /fun-emoji, /bottts, /avataaars, /pixel-art",
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                        cancellationToken: cancellationToken);
                }
                else if (patterns.Contains(emoji) && seed is null)
                {
                    //"Iltimos, buyruqdan keyin (seed) kiriting. Misol: /fun-emoji Ali"
                    await botClient.SendMessage(
                        chatId: update.Message.Chat.Id,
                        text: "Iltimos, buyruqdan keyin (seed) kiriting. Misol: /fun-emoji Ali",
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                        cancellationToken: cancellationToken);
                }
                else
                {
                    // "Iltimos, avatar olish uchun buyruqdan foydalaning."
                    await botClient.SendMessage(
                    chatId: update.Message.Chat.Id,
                    text: "Iltimos, avatar olish uchun to'g'ri buyruqdan foydalaning.",
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                    cancellationToken: cancellationToken);
                }
            }

        }
        await Task.CompletedTask;
    }
}