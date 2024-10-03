using FinancialAssistant.DataTransfer.Currency;

namespace FinancialAssistant.ExchangeRates.Implementation;

public class CurrencyRateManager : ICurrencyRateManager
{
    private static DateTime _lastFetchTime;
    private static List<GetCurrencyDto>? _currency;

    private readonly IFiatCurrencyService _fiatCurrencyService;
    private readonly ICryptoCurrencyService _cryptoCurrencyService;

    public CurrencyRateManager(IFiatCurrencyService fiatCurrencyService, 
        ICryptoCurrencyService cryptoCurrencyService)
    {
        _fiatCurrencyService = fiatCurrencyService;
        _cryptoCurrencyService = cryptoCurrencyService;
    }

    public List<GetCurrencyDto>? GetCurrencies(CancellationToken cancellationToken)
    {
        if (_lastFetchTime.AddHours(1) > DateTime.Now)
            if (_currency is { })
                return _currency;

        if (_fiatCurrencyService.GetFiatCurrency(cancellationToken) is not { } fiatCurrency)
            return default;

        if (_cryptoCurrencyService.GetCryptoCurrency(cancellationToken) is not { } cryptoCurrency)
            return default;

        _lastFetchTime = DateTime.Now;
        _currency = fiatCurrency.Concat(cryptoCurrency).ToList();
        
        return _currency;
    }

    public DateTime GetLastFetchTime(CancellationToken cancellationToken)
    {
        GetCurrencies(cancellationToken);
        return _lastFetchTime;
    }
}