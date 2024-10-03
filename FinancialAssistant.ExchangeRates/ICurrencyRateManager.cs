using FinancialAssistant.DataTransfer.Currency;

namespace FinancialAssistant.ExchangeRates;

public interface ICurrencyRateManager
{
    List<GetCurrencyDto>? GetCurrencies(CancellationToken cancellationToken);

    DateTime GetLastFetchTime(CancellationToken cancellationToken);
}