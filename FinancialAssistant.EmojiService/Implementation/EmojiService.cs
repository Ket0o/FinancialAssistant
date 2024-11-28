using System.Net.Http.Json;
using FinancialAssistant.DataTransfer.Emoji;
using FinancialAssistant.EmojiService.Options;
using Microsoft.Extensions.Options;

namespace FinancialAssistant.EmojiService.Implementation;

public class EmojiService : IEmojiService
{
    private const string RequestAddress = "https://emoji-api.com/categories/smileys-emotion?access_key=";
    
    private static readonly object Lock = new();
    private static readonly string[] EmojiSubGroups =
    [
        "emotion",
        "face-smiling",
        "cat-face",
        "monkey-face"
    ];
    private readonly HttpClient _httpClient;
    private readonly string _accessKey;
    private static List<GetEmojiDto>? _emojis;

    public EmojiService(
        HttpClient httpClient,
        IOptions<EmojiSettings> options)
    {
        var settings = options.Value;
        _httpClient = httpClient;
        _accessKey = settings.AccessKey;
    }
    
    public string? GetRandomPositiveEmoji(CancellationToken cancellationToken)
    {
        if (GetPositiveEmojis(cancellationToken) is not { } emojis)
            return null;

        var arrayEmojis = emojis.ToArray();
        
        return arrayEmojis[Random.Shared.Next(arrayEmojis.Length)].Character;
    }

    private List<GetEmojiDto>? GetPositiveEmojis(CancellationToken cancellationToken)
    {
        if (_emojis is { })
            return _emojis;

        lock (Lock)
        {
            if (_emojis is { })
                return _emojis;
            
            using var response =
                _httpClient.Send(new HttpRequestMessage(HttpMethod.Get, new Uri(RequestAddress + _accessKey)),
                    cancellationToken);

            if (!response.IsSuccessStatusCode)
                return default;

            var emojis 
                = response!.Content.ReadFromJsonAsync<IEnumerable<GetEmojiDto>>(cancellationToken).Result.ToArray();

            _emojis = emojis
                .Where(emoji => EmojiSubGroups.Contains(emoji.SubGroup, StringComparer.InvariantCultureIgnoreCase))
                .ToList();
        }

        return new List<GetEmojiDto>();
    }
}