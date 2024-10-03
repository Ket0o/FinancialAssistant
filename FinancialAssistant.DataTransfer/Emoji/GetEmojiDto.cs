namespace FinancialAssistant.DataTransfer.Emoji;

public record GetEmojiDto(string Slug, string Character, string UnicodeName, string CodePoint, string Group, string SubGroup);