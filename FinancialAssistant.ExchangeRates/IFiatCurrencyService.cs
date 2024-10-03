using FinancialAssistant.DataTransfer.Currency;

namespace FinancialAssistant.ExchangeRates;

public interface IFiatCurrencyService
{
    List<GetCurrencyDto>? GetFiatCurrency(CancellationToken cancellationToken);
}