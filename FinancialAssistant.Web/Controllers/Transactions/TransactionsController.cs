using FinancialAssistant.Web.Controllers.Transactions.Responses;
using FinancialAssistant.Web.Mapping.Transactions;
using FinancialAssistant.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAssistant.Web.Controllers.Transactions;

[Authorize]
[ApiController]
[Route("[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetTransactionResponse>> GetTransaction(long id, CancellationToken cancellationToken)
    {
        var result = await _transactionService.GetTransaction(id, cancellationToken);
        return result.IsT0 ? Ok(result.AsT0.Value.ToGetTransactionResponse()) : NotFound();
    }
}