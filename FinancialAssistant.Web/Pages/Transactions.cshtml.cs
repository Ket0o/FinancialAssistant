using FinancialAssistant.DataAccess.Model;
using FinancialAssistant.DataTransfer.Transaction;
using FinancialAssistant.Web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinancialAssistant.Web.Pages
{
    public class TransactionsModel : PageModel
    {
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService;

        public TransactionsModel(ITransactionService transactionService, 
            IAccountService accountService)
        {
            _transactionService = transactionService;
            _accountService = accountService;
        }

        // Свойства для передачи данных на страницу
        public string[] AccountNames { get; set; } 
        public DateTime CurrentMonth { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal LastExpenseAmount { get; set; }
        public decimal AccountBalance { get; set; } = 10000;
        public long LastExpenseId { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal LastIncomeAmount { get; set; }
        public long LastIncomeId { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public bool HasPreviousMonth { get; set; }
        public Transaction CurrentTransaction { get; set; }

        public async Task OnGet(CancellationToken cancellationToken)
        {
            CurrentMonth = DateTime.Now;
            var accounts = await _accountService.GetAllAccounts(cancellationToken);
            AccountNames = accounts.Select(account => account.Name).ToArray();
            await LoadTransactions( CurrentMonth, cancellationToken);
        }

        public async Task UpdateTransaction(UpdateTransaction transaction, CancellationToken cancellationToken)
        {
            await _transactionService.ChangeTransaction(
                new UpdateTransactionDto(transaction.Id, transaction.AccountId, transaction.CategoryId,
                    transaction.Amount, transaction.TransactionDate, transaction.Description), cancellationToken);
        }

        private async Task LoadTransactions(DateTime month, CancellationToken cancellationToken)
        {
            // Здесь вы можете заменить на реальную логику получения данных из базы данных
            if (await GetTransactions(month, cancellationToken) is not {} transactions)
                return;
            
            Transactions = transactions;
            TotalExpenses = Transactions.Where(t => t.Amount < 0).Sum(t => t.Amount);
            TotalIncome = Transactions.Where(t => t.Amount > 0).Sum(t => t.Amount);
            LastExpenseAmount = Transactions.Where(t => t.Amount < 0).OrderByDescending(t => t.Date).FirstOrDefault()?.Amount ?? 0;
            LastExpenseId = Transactions.Where(t => t.Amount < 0).OrderByDescending(t => t.Date).FirstOrDefault()?.Id ?? 0;
            LastIncomeAmount = Transactions.Where(t => t.Amount > 0).OrderByDescending(t => t.Date).FirstOrDefault()?.Amount ?? 0;
            LastIncomeId = Transactions.Where(t => t.Amount > 0).OrderByDescending(t => t.Date).FirstOrDefault()?.Id ?? 0;
            HasPreviousMonth = CheckForPreviousMonth(month);
        }

        private async Task<List<Transaction>?> GetTransactions(DateTime month, CancellationToken cancellationToken )
        {
            if (await _transactionService.GetMonthAndAccountTransactions(month.Month, cancellationToken) is not { } transactions)
                return null;

            return transactions.Select(transaction => new Transaction
            {
                Id = transaction.Id,
                Name = transaction.Name,
                Amount = decimal.Abs(transaction.Amount),
                Date = transaction.TransactionDate,
                Category = transaction.CategoryName,
                TransactionType = transaction.Amount > 0 ? "Доход" : "Расход",
                CategoryColor = transaction.CategoryColor,
                Description = transaction.Description,
                Account = transaction.AccountName
            }).ToList();
        }

        private bool CheckForPreviousMonth(DateTime month)
        {
            DateTime previousMonth = month.AddMonths(-1);
            return true;
        }
    }

    public class Transaction
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateOnly Date { get; set; }
        public string Category { get; set; }= string.Empty;
        public string TransactionType { get; set; }= string.Empty;
        public string CategoryColor { get; set; }= string.Empty;
        public string Account { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class UpdateTransaction
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public long CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateOnly TransactionDate { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
    }
}
