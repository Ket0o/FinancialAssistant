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

        public IndexModel(IEmojiService emojiService, 
            ICurrencyRateManager currencyRateManager, 
            IGreetingsService greetingsService)
        {
            _emojiService = emojiService;
            _currencyRateManager = currencyRateManager;
            _greetingsService = greetingsService;
        }

        public string GreetingMessage { get; set; }
        public string? Emoji { get; set; }
        
        public DateTime LastCurrenciesUpdateTime { get; set; }
        public IEnumerable<GetCurrencyDto>? Currencies { get; set; }
        public List<Operation> LastOperations { get; set; }
        
        public void OnGet(CancellationToken cancellationToken)
        {
            GreetingMessage = _greetingsService.GetGreetings();

            if (_emojiService.GetRandomPositiveEmoji(cancellationToken) is { } emoji)
                Emoji = emoji;

            if (_currencyRateManager.GetCurrencies(cancellationToken) is { } currencies)
                Currencies = currencies;

            LastCurrenciesUpdateTime = _currencyRateManager.GetLastFetchTime(cancellationToken); 
            
            // Последние операции
            LastOperations = new List<Operation>
            {
                new Operation { Place = "Кафе", Amount = 500, Date = DateTime.Now.AddDays(-1), Color = "#FF5733" },
                new Operation { Place = "Супермаркет", Amount = 1500, Date = DateTime.Now.AddDays(-2), Color = "#33FF57" },
                new Operation { Place = "Супермаркет", Amount = 1500, Date = DateTime.Now.AddDays(-2), Color = "#33FF57" },
                new Operation { Place = "Супермаркет", Amount = 1500, Date = DateTime.Now.AddDays(-2), Color = "#33FF57" },
                new Operation { Place = "Супермаркет", Amount = 1500, Date = DateTime.Now.AddDays(-2), Color = "#33FF57" },
                new Operation { Place = "Супермаркет", Amount = 1500, Date = DateTime.Now.AddDays(-2), Color = "#33FF57" },
                new Operation { Place = "Супермаркет", Amount = 1500, Date = DateTime.Now.AddDays(-2), Color = "#33FF57" },
                new Operation { Place = "Супермаркет", Amount = 1500, Date = DateTime.Now.AddDays(-2), Color = "#33FF57" },
                new Operation { Place = "Супермаркет", Amount = 1500, Date = DateTime.Now.AddDays(-2), Color = "#33FF57" },
                new Operation { Place = "Супермаркет", Amount = 1500, Date = DateTime.Now.AddDays(-2), Color = "#33FF57" }
                // Добавьте больше операций по мере необходимости
            };
        }
    }

    public class Operation
    {
        public string Place { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Color { get; set; }
    }
}
