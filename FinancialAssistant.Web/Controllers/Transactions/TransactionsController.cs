using FinancialAssistant.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAssistant.Web.Controllers.Transactions;

[Authorize]
[ApiController]
[Route("[action]")]
public class TransactionsController : ControllerBase
{
    private ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }
}