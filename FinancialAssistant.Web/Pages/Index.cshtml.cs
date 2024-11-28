using FinancialAssistant.DataTransfer.Currency;
using FinancialAssistant.EmojiService;
using FinancialAssistant.ExchangeRates;
using FinancialAssistant.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinancialAssistant.Web.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IEmojiService _emojiService;
        private readonly ICurrencyRateManager _currencyRateManager;
        private readonly IGreetingsService _greetingsService;
        private readonly ITransactionService _transactionService;

        public IndexModel(
            IEmojiService emojiService, 
            ICurrencyRateManager currencyRateManager, 
            IGreetingsService greetingsService, 
            ITransactionService transactionService)
        {
            _emojiService = emojiService;
            _currencyRateManager = currencyRateManager;
            _greetingsService = greetingsService;
            _transactionService = transactionService;
        }

        public string GreetingMessage { get; set; }
        public string? Emoji { get; set; }
        
        public DateTime LastCurrenciesUpdateTime { get; set; }
        public IEnumerable<GetCurrencyDto>? Currencies { get; set; }
        public List<Operation> LastOperations { get; set; }
        
        public async Task OnGet(CancellationToken cancellationToken)
        {
            GreetingMessage = _greetingsService.GetGreetings();

            if (_emojiService.GetRandomPositiveEmoji(cancellationToken) is { } emoji)
                Emoji = emoji;

            if (_currencyRateManager.GetCurrencies(cancellationToken) is { } currencies)
                Currencies = currencies;

            LastCurrenciesUpdateTime = _currencyRateManager.GetLastFetchTime(cancellationToken); 
            
            if (await _transactionService.GetLastTenTransactions(cancellationToken) is not {} transactions)
                return;

            LastOperations = transactions.Select(transaction => new Operation
            {
                Amount = transaction.Amount,
                Name = transaction.Name,
                Date = transaction.TransactionDate,
                Color = transaction.CategoryColor
            }).ToList();
        }
    }

    public class Operation
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateOnly Date { get; set; }
        public string Color { get; set; }
    }
}
