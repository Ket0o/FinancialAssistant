using FinancialAssistant.DataTransfer.Account;
using FinancialAssistant.DataTransfer.Transaction;
using FinancialAssistant.Web.Controllers.Transactions.Requests;
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

    [HttpGet]
    public async Task<ActionResult<List<GetTransactionResponse>>> GetAllTransactions(
        [FromQuery]GetTransactionRequest request, CancellationToken cancellationToken)
    {
        var result = await _transactionService.GetAllTransactions(
            new TransactionFilterDto(request.AccountId, request.CategoryId, request.YearMonth, request.SortBy),
            cancellationToken: cancellationToken);

        return result.IsT0
            ? Ok(result.AsT0.Value.Select(transaction => transaction.ToGetTransactionResponse()))
            : NotFound();
    }

    [HttpPost("Create")]
    public async Task<ActionResult> CreateTransaction(CreateTransactionRequest request, 
        CancellationToken cancellationToken)
    {
        await _transactionService.AddTransaction(
            new AddTransactionDto(request.AccountId, request.CategoryId, request.Name, request.Amount, 
                request.TransactionDate, request.Description), cancellationToken);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTransaction(long id, UpdateTransactionRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _transactionService.ChangeTransaction(
            new UpdateTransactionDto(id, request.AccountId, request.Name, request.CategoryId,  request.Amount,
                request.TransactionDate, request.Description), cancellationToken);
        
        return result.IsT0 ? Ok() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTransaction(long id, CancellationToken cancellationToken)
    {
        var result = await _transactionService.DeleteTransaction(id, cancellationToken);
        return result.IsT0 ? Ok() : NotFound();
    }
}