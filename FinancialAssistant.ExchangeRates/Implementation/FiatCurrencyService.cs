using FinancialAssistant.DataTransfer.Currency;
using FinancialAssistant.ExchangeRates.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace FinancialAssistant.ExchangeRates.Implementation;

public class FiatCurrencyService : IFiatCurrencyService
{
    private const string CurrencyGroup = "conversion_rates";
    private const string RubleNamingCurrency = "RUB";
    private const string EuroNamingCurrency = "EUR";
    
    private static List<GetCurrencyDto>? _currency;
    private static readonly object Lock = new();
    private readonly HttpClient _httpClient;
    private readonly string _accessKey;
    
    public FiatCurrencyService(
        HttpClient httpClient, 
        IOptions<FiatCurrencySettings> options)
    {
        _httpClient = httpClient;
        var settings = options.Value;
        _accessKey = settings.AccessKey;
    }

    public List<GetCurrencyDto>? GetFiatCurrency(CancellationToken cancellationToken)
    {
        lock (Lock)
        {
            using var response =
                _httpClient.Send(new HttpRequestMessage(HttpMethod.Get, 
                new Uri($"https://v6.exchangerate-api.com/v6/{_accessKey}/latest/USD")),
                    cancellationToken);

            if (!response.IsSuccessStatusCode)
                return default;
            
            var jObject = JObject.Parse(response.Content.ReadAsStringAsync(cancellationToken).Result);

            if (jObject is not { })
                return default;
            
            _currency = new List<GetCurrencyDto>
            {
                new (Icon: default, Name: RubleNamingCurrency, Value: jObject![CurrencyGroup][RubleNamingCurrency].Value<decimal>()),
                new (Icon: default, Name: EuroNamingCurrency, Value: jObject![CurrencyGroup][EuroNamingCurrency].Value<decimal>())
            };
        }

        return _currency;
    }
}