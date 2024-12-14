using Microsoft.AspNetCore.Mvc;

namespace FinancialAssistant.Web.Controllers.Transactions.Requests;

public record UpdateTransactionRequest(long AccountId, string Name, long CategoryId, decimal Amount, 
    DateOnly TransactionDate, string? Description);