using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinancialAssistant.Web.Pages
{
    public class TransactionsModel : PageModel
    {
        // Свойства для передачи данных на страницу
        public string[] AccountNames { get; set; } = {"Личный счет", "Копилка", "Деньги на квартир31231231у"}; // Пример названия счета
        public DateTime CurrentMonth { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal LastExpenseAmount { get; set; }
        public decimal AccountBalance { get; set; } = 10000;
        public int LastExpenseId { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal LastIncomeAmount { get; set; }
        public int LastIncomeId { get; set; }
        public List<Transaction> Transactions { get; set; }
        public bool HasPreviousMonth { get; set; }

        public void OnGet(int? monthOffset = 0)
        {
            CurrentMonth = DateTime.Now.AddMonths(monthOffset ?? 0);
            LoadTransactions(CurrentMonth);
        }

        private void LoadTransactions(DateTime month)
        {
            // Здесь вы можете заменить на реальную логику получения данных из базы данных
            Transactions = GetTestTransactions(month);
            TotalExpenses = Transactions.Where(t => t.Amount < 0).Sum(t => t.Amount);
            TotalIncome = Transactions.Where(t => t.Amount > 0).Sum(t => t.Amount);
            LastExpenseAmount = Transactions.Where(t => t.Amount < 0).OrderByDescending(t => t.Date).FirstOrDefault()?.Amount ?? 0;
            LastExpenseId = Transactions.Where(t => t.Amount < 0).OrderByDescending(t => t.Date).FirstOrDefault()?.Id ?? 0;
            LastIncomeAmount = Transactions.Where(t => t.Amount > 0).OrderByDescending(t => t.Date).FirstOrDefault()?.Amount ?? 0;
            LastIncomeId = Transactions.Where(t => t.Amount > 0).OrderByDescending(t => t.Date).FirstOrDefault()?.Id ?? 0;
            HasPreviousMonth = CheckForPreviousMonth(month);
        }

        private List<Transaction> GetTestTransactions(DateTime month)
        {
            
        }

        private bool CheckForPreviousMonth(DateTime month)
        {
            DateTime previousMonth = month.AddMonths(-1);
            return true;
        }
    }

    public class Transaction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public string TransactionType { get; set; }
        public string CategoryColor { get; set; }
    }
}
