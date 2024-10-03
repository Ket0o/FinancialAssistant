using FinancialAssistant.DataTransfer.Currency;

namespace FinancialAssistant.ExchangeRates;

public interface ICryptoCurrencyService
{
    List<GetCurrencyDto>? GetCryptoCurrency(CancellationToken cancellationToken);
}