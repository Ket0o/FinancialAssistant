using FinancialAssistant.DataTransfer.Emoji;

namespace FinancialAssistant.EmojiService;

public interface IEmojiService
{
    string? GetRandomPositiveEmoji(CancellationToken cancellationToken);
}