using FinancialAssistant.DataTransfer.Currency;
using Newtonsoft.Json.Linq;

namespace FinancialAssistant.ExchangeRates.Implementation;

public class CryptoCurrencyService : ICryptoCurrencyService
{
    private const string CurrencyGroup = "price_usd";
    private const string BitcoinNamingCurrency = "BTC";
    private const string EthereumNamingCurrency = "ETH";
    private const string RequestAddress = "https://api.coinlore.net/api/ticker/?id=90,80";
    
    private static List<GetCurrencyDto>? _currency;
    private static readonly object Lock = new();
    private readonly HttpClient _httpClient;
    
    public CryptoCurrencyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public List<GetCurrencyDto>? GetCryptoCurrency(CancellationToken cancellationToken)
    {
        lock (Lock)
        {
            using var response =
                _httpClient.Send(new HttpRequestMessage(HttpMethod.Get, new Uri(RequestAddress)),
                    cancellationToken);

            if (!response.IsSuccessStatusCode)
                return default;

            var responseContent = response.Content.ReadAsStringAsync(cancellationToken).Result;
            
            var jArray = JArray.Parse(responseContent);

            if (jArray is not { })
                return default;
            
            _currency = new List<GetCurrencyDto>
            {
                new (Icon: default, Name: BitcoinNamingCurrency, 
                    Value: Math.Round(1/jArray![0][CurrencyGroup].Value<decimal>(), 8)),
                new (Icon: default, Name: EthereumNamingCurrency, 
                    Value: Math.Round(1/jArray![1][CurrencyGroup].Value<decimal>(), 8))
            };
        }

        return _currency;
    }
}