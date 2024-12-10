using FinancialAssistant.DataAccess.Model;
using FinancialAssistant.DataTransfer.Transaction;

namespace FinancialAssistant.Web.Mapping.Transactions;

public static class TransactionsMapping
{
    public static Transaction ToModel(this UpdateTransactionDto updateTransaction)
        => new ()
        {
            AccountId = updateTransaction.Id,
            CategoryId = updateTransaction.CategoryId,
            Amount = updateTransaction.Amount,
            TransactionDate = updateTransaction.TransactionDate,
            Description = updateTransaction.Description
        };

    public static Transaction ToModel(this AddTransactionDto addTransaction)
        => new()
        {
            AccountId = addTransaction.AccountId,
            CategoryId = addTransaction.CategoryId,
            Amount = addTransaction.Amount,
            TransactionDate = addTransaction.TransactionDate,
            Description = addTransaction.Description,
            CreatedAt = DateTime.Now
        };

    public static GetTransactionDto ToGetTransactionDto(this Transaction transaction)
        => new (transaction.Id, transaction.Name, transaction.AccountId, transaction.CategoryId,
            transaction.Amount, transaction.TransactionDate, transaction.Description, transaction.CreatedAt,
            transaction.Account.Name, transaction.Category.Name, transaction.Category.Color);
}