using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinancialAssistant.Web.Pages
{
    public class TransactionsModel : PageModel
    {
        // Свойства для передачи данных на страницу
        public string AccountName { get; set; } = "Личный счет"; // Пример названия счета
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
            // Вычисляем текущий месяц с учетом смещения
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
            // Генерация тестовых данных
            return new List<Transaction>
            {
                new Transaction { Id = 1, Name = "Зарплата", Amount = 5000, Date = new DateTime(month.Year, month.Month, 1), Category = "Зарплата", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 2, Name = "Кофе", Amount = -150, Date = new DateTime(month.Year, month.Month, 2), Category = "Еда", TransactionType = "Расход", CategoryColor = "#FF5722" },
                new Transaction { Id = 3, Name = "Аренда", Amount = -2000, Date = new DateTime(month.Year, month.Month, 5), Category = "Жилищные расходы", TransactionType = "Расход", CategoryColor = "#2196F3" },
                new Transaction { Id = 4, Name = "Ужин с друзьями", Amount = -800, Date = new DateTime(month.Year, month.Month, 10), Category = "Развлечения", TransactionType = "Расход", CategoryColor = "#FF9800" },
                new Transaction { Id = 5, Name = "Бонус", Amount = 1000, Date = new DateTime(month.Year, month.Month, 15), Category = "Доход", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 6, Name = "Кино", Amount = -500, Date = new DateTime(month.Year, month.Month, 20), Category = "Развлечения", TransactionType = "Расход", CategoryColor = "#FF9800" },
                new Transaction { Id = 7, Name = "Коммунальные услуги", Amount = -300, Date = new DateTime(month.Year, month.Month, 25), Category = "Жилищные расходы", TransactionType = "Расход", CategoryColor = "#2196F3" },
                new Transaction { Id = 8, Name = "Фриланс", Amount = 1200, Date = new DateTime(month.Year, month.Month, 28), Category = "Доход", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 1, Name = "Зарплата", Amount = 5000, Date = new DateTime(month.Year, month.Month, 1), Category = "Зарплата", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 2, Name = "Кофе", Amount = -150, Date = new DateTime(month.Year, month.Month, 2), Category = "Еда", TransactionType = "Расход", CategoryColor = "#FF5722" },
                new Transaction { Id = 3, Name = "Аренда", Amount = -2000, Date = new DateTime(month.Year, month.Month, 5), Category = "Жилищные расходы", TransactionType = "Расход", CategoryColor = "#2196F3" },
                new Transaction { Id = 4, Name = "Ужин с друзьями", Amount = -800, Date = new DateTime(month.Year, month.Month, 10), Category = "Развлечения", TransactionType = "Расход", CategoryColor = "#FF9800" },
                new Transaction { Id = 5, Name = "Бонус", Amount = 1000, Date = new DateTime(month.Year, month.Month, 15), Category = "Доход", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 6, Name = "Кино", Amount = -500, Date = new DateTime(month.Year, month.Month, 20), Category = "Развлечения", TransactionType = "Расход", CategoryColor = "#FF9800" },
                new Transaction { Id = 7, Name = "Коммунальные услуги", Amount = -300, Date = new DateTime(month.Year, month.Month, 25), Category = "Жилищные расходы", TransactionType = "Расход", CategoryColor = "#2196F3" },
                new Transaction { Id = 8, Name = "Фриланс", Amount = 1200, Date = new DateTime(month.Year, month.Month, 28), Category = "Доход", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 1, Name = "Зарплата", Amount = 5000, Date = new DateTime(month.Year, month.Month, 1), Category = "Зарплата", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 2, Name = "Кофе", Amount = -150, Date = new DateTime(month.Year, month.Month, 2), Category = "Еда", TransactionType = "Расход", CategoryColor = "#FF5722" },
                new Transaction { Id = 3, Name = "Аренда", Amount = -2000, Date = new DateTime(month.Year, month.Month, 5), Category = "Жилищные расходы", TransactionType = "Расход", CategoryColor = "#2196F3" },
                new Transaction { Id = 4, Name = "Ужин с друзьями", Amount = -800, Date = new DateTime(month.Year, month.Month, 10), Category = "Развлечения", TransactionType = "Расход", CategoryColor = "#FF9800" },
                new Transaction { Id = 5, Name = "Бонус", Amount = 1000, Date = new DateTime(month.Year, month.Month, 15), Category = "Доход", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 6, Name = "Кино", Amount = -500, Date = new DateTime(month.Year, month.Month, 20), Category = "Развлечения", TransactionType = "Расход", CategoryColor = "#FF9800" },
                new Transaction { Id = 7, Name = "Коммунальные услуги", Amount = -300, Date = new DateTime(month.Year, month.Month, 25), Category = "Жилищные расходы", TransactionType = "Расход", CategoryColor = "#2196F3" },
                new Transaction { Id = 8, Name = "Фриланс", Amount = 1200, Date = new DateTime(month.Year, month.Month, 28), Category = "Доход", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 1, Name = "Зарплата", Amount = 5000, Date = new DateTime(month.Year, month.Month, 1), Category = "Зарплата", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 2, Name = "Кофе", Amount = -150, Date = new DateTime(month.Year, month.Month, 2), Category = "Еда", TransactionType = "Расход", CategoryColor = "#FF5722" },
                new Transaction { Id = 3, Name = "Аренда", Amount = -2000, Date = new DateTime(month.Year, month.Month, 5), Category = "Жилищные расходы", TransactionType = "Расход", CategoryColor = "#2196F3" },
                new Transaction { Id = 4, Name = "Ужин с друзьями", Amount = -800, Date = new DateTime(month.Year, month.Month, 10), Category = "Развлечения", TransactionType = "Расход", CategoryColor = "#FF9800" },
                new Transaction { Id = 5, Name = "Бонус", Amount = 1000, Date = new DateTime(month.Year, month.Month, 15), Category = "Доход", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 6, Name = "Кино", Amount = -500, Date = new DateTime(month.Year, month.Month, 20), Category = "Развлечения", TransactionType = "Расход", CategoryColor = "#FF9800" },
                new Transaction { Id = 7, Name = "Коммунальные услуги", Amount = -300, Date = new DateTime(month.Year, month.Month, 25), Category = "Жилищные расходы", TransactionType = "Расход", CategoryColor = "#2196F3" },
                new Transaction { Id = 8, Name = "Фриланс", Amount = 1200, Date = new DateTime(month.Year, month.Month, 28), Category = "Доход", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 1, Name = "Зарплата", Amount = 5000, Date = new DateTime(month.Year, month.Month, 1), Category = "Зарплата", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 2, Name = "Кофе", Amount = -150, Date = new DateTime(month.Year, month.Month, 2), Category = "Еда", TransactionType = "Расход", CategoryColor = "#FF5722" },
                new Transaction { Id = 3, Name = "Аренда", Amount = -2000, Date = new DateTime(month.Year, month.Month, 5), Category = "Жилищные расходы", TransactionType = "Расход", CategoryColor = "#2196F3" },
                new Transaction { Id = 4, Name = "Ужин с друзьями", Amount = -800, Date = new DateTime(month.Year, month.Month, 10), Category = "Развлечения", TransactionType = "Расход", CategoryColor = "#FF9800" },
                new Transaction { Id = 5, Name = "Бонус", Amount = 1000, Date = new DateTime(month.Year, month.Month, 15), Category = "Доход", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 6, Name = "Кино", Amount = -500, Date = new DateTime(month.Year, month.Month, 20), Category = "Развлечения", TransactionType = "Расход", CategoryColor = "#FF9800" },
                new Transaction { Id = 7, Name = "Коммунальные услуги", Amount = -300, Date = new DateTime(month.Year, month.Month, 25), Category = "Жилищные расходы", TransactionType = "Расход", CategoryColor = "#2196F3" },
                new Transaction { Id = 8, Name = "Фриланс", Amount = 1200, Date = new DateTime(month.Year, month.Month, 28), Category = "Доход", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 1, Name = "Зарплата", Amount = 5000, Date = new DateTime(month.Year, month.Month, 1), Category = "Зарплата", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 2, Name = "Кофе", Amount = -150, Date = new DateTime(month.Year, month.Month, 2), Category = "Еда", TransactionType = "Расход", CategoryColor = "#FF5722" },
                new Transaction { Id = 3, Name = "Аренда", Amount = -2000, Date = new DateTime(month.Year, month.Month, 5), Category = "Жилищные расходы", TransactionType = "Расход", CategoryColor = "#2196F3" },
                new Transaction { Id = 4, Name = "Ужин с друзьями", Amount = -800, Date = new DateTime(month.Year, month.Month, 10), Category = "Развлечения", TransactionType = "Расход", CategoryColor = "#FF9800" },
                new Transaction { Id = 5, Name = "Бонус", Amount = 1000, Date = new DateTime(month.Year, month.Month, 15), Category = "Доход", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 6, Name = "Кино", Amount = -500, Date = new DateTime(month.Year, month.Month, 20), Category = "Развлечения", TransactionType = "Расход", CategoryColor = "#FF9800" },
                new Transaction { Id = 7, Name = "Коммунальные услуги", Amount = -300, Date = new DateTime(month.Year, month.Month, 25), Category = "Жилищные расходы", TransactionType = "Расход", CategoryColor = "#2196F3" },
                new Transaction { Id = 8, Name = "Фриланс", Amount = 1200, Date = new DateTime(month.Year, month.Month, 28), Category = "Доход", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 1, Name = "Зарплата", Amount = 5000, Date = new DateTime(month.Year, month.Month, 1), Category = "Зарплата", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 2, Name = "Кофе", Amount = -150, Date = new DateTime(month.Year, month.Month, 2), Category = "Еда", TransactionType = "Расход", CategoryColor = "#FF5722" },
                new Transaction { Id = 3, Name = "Аренда", Amount = -2000, Date = new DateTime(month.Year, month.Month, 5), Category = "Жилищные расходы", TransactionType = "Расход", CategoryColor = "#2196F3" },
                new Transaction { Id = 4, Name = "Ужин с друзьями", Amount = -800, Date = new DateTime(month.Year, month.Month, 10), Category = "Развлечения", TransactionType = "Расход", CategoryColor = "#FF9800" },
                new Transaction { Id = 5, Name = "Бонус", Amount = 1000, Date = new DateTime(month.Year, month.Month, 15), Category = "Доход", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 6, Name = "Кино", Amount = -500, Date = new DateTime(month.Year, month.Month, 20), Category = "Развлечения", TransactionType = "Расход", CategoryColor = "#FF9800" },
                new Transaction { Id = 7, Name = "Коммунальные услуги", Amount = -300, Date = new DateTime(month.Year, month.Month, 25), Category = "Жилищные расходы", TransactionType = "Расход", CategoryColor = "#2196F3" },
                new Transaction { Id = 8, Name = "Фриланс", Amount = 1200, Date = new DateTime(month.Year, month.Month, 28), Category = "Доход", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 1, Name = "Зарплата", Amount = 5000, Date = new DateTime(month.Year, month.Month, 1), Category = "Зарплата", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 2, Name = "Кофе", Amount = -150, Date = new DateTime(month.Year, month.Month, 2), Category = "Еда", TransactionType = "Расход", CategoryColor = "#FF5722" },
                new Transaction { Id = 3, Name = "Аренда", Amount = -2000, Date = new DateTime(month.Year, month.Month, 5), Category = "Жилищные расходы", TransactionType = "Расход", CategoryColor = "#2196F3" },
                new Transaction { Id = 4, Name = "Ужин с друзьями", Amount = -800, Date = new DateTime(month.Year, month.Month, 10), Category = "Развлечения", TransactionType = "Расход", CategoryColor = "#FF9800" },
                new Transaction { Id = 5, Name = "Бонус", Amount = 1000, Date = new DateTime(month.Year, month.Month, 15), Category = "Доход", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 6, Name = "Кино", Amount = -500, Date = new DateTime(month.Year, month.Month, 20), Category = "Развлечения", TransactionType = "Расход", CategoryColor = "#FF9800" },
                new Transaction { Id = 7, Name = "Коммунальные услуги", Amount = -300, Date = new DateTime(month.Year, month.Month, 25), Category = "Жилищные расходы", TransactionType = "Расход", CategoryColor = "#2196F3" },
                new Transaction { Id = 8, Name = "Фриланс", Amount = 1200, Date = new DateTime(month.Year, month.Month, 28), Category = "Доход", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 1, Name = "Зарплата", Amount = 5000, Date = new DateTime(month.Year, month.Month, 1), Category = "Зарплата", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 2, Name = "Кофе", Amount = -150, Date = new DateTime(month.Year, month.Month, 2), Category = "Еда", TransactionType = "Расход", CategoryColor = "#FF5722" },
                new Transaction { Id = 3, Name = "Аренда", Amount = -2000, Date = new DateTime(month.Year, month.Month, 5), Category = "Жилищные расходы", TransactionType = "Расход", CategoryColor = "#2196F3" },
                new Transaction { Id = 4, Name = "Ужин с друзьями", Amount = -800, Date = new DateTime(month.Year, month.Month, 10), Category = "Развлечения", TransactionType = "Расход", CategoryColor = "#FF9800" },
                new Transaction { Id = 5, Name = "Бонус", Amount = 1000, Date = new DateTime(month.Year, month.Month, 15), Category = "Доход", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 6, Name = "Кино", Amount = -500, Date = new DateTime(month.Year, month.Month, 20), Category = "Развлечения", TransactionType = "Расход", CategoryColor = "#FF9800" },
                new Transaction { Id = 7, Name = "Коммунальные услуги", Amount = -300, Date = new DateTime(month.Year, month.Month, 25), Category = "Жилищные расходы", TransactionType = "Расход", CategoryColor = "#2196F3" },
                new Transaction { Id = 8, Name = "Фриланс", Amount = 1200, Date = new DateTime(month.Year, month.Month, 28), Category = "Доход", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 1, Name = "Зарплата", Amount = 5000, Date = new DateTime(month.Year, month.Month, 1), Category = "Зарплата", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 2, Name = "Кофе", Amount = -150, Date = new DateTime(month.Year, month.Month, 2), Category = "Еда", TransactionType = "Расход", CategoryColor = "#FF5722" },
                new Transaction { Id = 3, Name = "Аренда", Amount = -2000, Date = new DateTime(month.Year, month.Month, 5), Category = "Жилищные расходы", TransactionType = "Расход", CategoryColor = "#2196F3" },
                new Transaction { Id = 4, Name = "Ужин с друзьями", Amount = -800, Date = new DateTime(month.Year, month.Month, 10), Category = "Развлечения", TransactionType = "Расход", CategoryColor = "#FF9800" },
                new Transaction { Id = 5, Name = "Бонус", Amount = 1000, Date = new DateTime(month.Year, month.Month, 15), Category = "Доход", TransactionType = "Доход", CategoryColor = "#4CAF50" },
                new Transaction { Id = 6, Name = "Кино", Amount = -500, Date = new DateTime(month.Year, month.Month, 20), Category = "Развлечения", TransactionType = "Расход", CategoryColor = "#FF9800" },
                new Transaction { Id = 7, Name = "Коммунальные услуги", Amount = -300, Date = new DateTime(month.Year, month.Month, 25), Category = "Жилищные расходы", TransactionType = "Расход", CategoryColor = "#2196F3" },
                new Transaction { Id = 8, Name = "Фриланс", Amount = 1200, Date = new DateTime(month.Year, month.Month, 28), Category = "Доход", TransactionType = "Доход", CategoryColor = "#4CAF50" },
            };
        }

        private bool CheckForPreviousMonth(DateTime month)
        {
            DateTime previousMonth = month.AddMonths(-1);
            // Здесь вы можете добавить логику для проверки наличия транзакций за предыдущий месяц
            // Для тестирования мы просто вернем true, чтобы показать, что есть возможность переключения
            // Для тестирования мы просто вернем true, чтобы показать, что есть возможность переключения
            return true; // Замените на реальную проверку, если необходимо
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
