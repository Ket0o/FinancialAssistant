using FinancialAssistant.DataAccess.Model;
using FinancialAssistant.DataTransfer.Transaction;
using FinancialAssistant.Web.Controllers.Transactions.Responses;

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

    public static GetTransactionResponse ToGetTransactionResponse(this GetTransactionDto transactionDto)
        => new(transactionDto.Id, transactionDto.Name, transactionDto.AccountId, transactionDto.CategoryId,
            transactionDto.Amount, transactionDto.TransactionDate, transactionDto.Description, transactionDto.CreatedAt,
            transactionDto.AccountName, transactionDto.CategoryName, transactionDto.CategoryColor);
}