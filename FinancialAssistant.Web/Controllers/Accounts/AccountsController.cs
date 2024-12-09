using FinancialAssistant.DataTransfer.Account;
using FinancialAssistant.Web.Controllers.Accounts.Requests;
using FinancialAssistant.Web.Controllers.Accounts.Responses;
using FinancialAssistant.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAssistant.Web.Controllers.Accounts;

[ApiController]
[Authorize]
[Route("[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("Create")]
    public async Task<ActionResult<CreateAccountResponse>> Create([FromBody]string name, CancellationToken cancellationToken)
    {
        var result = await _accountService.AddAccount(name, cancellationToken);
        return result.IsT0 ? Ok(new CreateAccountResponse(result.AsT0.Value)) : BadRequest(result.AsT1);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAccount(long id, CancellationToken cancellationToken)
    {
        var result = await _accountService.DeleteAccount(id, cancellationToken);
        return result.IsT0 ? Ok(result.AsT0) : BadRequest(result.AsT1);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAccount(long id, UpdateAccountRequest request, 
        CancellationToken cancellationToken)
    {
        var result =
            await _accountService.EditAccount(new UpdateAccountDto(id, request.Name, request.IsDefault),
                cancellationToken);
        return result.IsT0 ? Ok(result.AsT0) : BadRequest(result.AsT1);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetAccountResponse>> GetAccount(long id, CancellationToken cancellationToken)
    {
        var result = await _accountService.GetAccount(id, cancellationToken);
        return result.IsT0 ? Ok(new GetAccountResponse(
            result.AsT0.Value.Id,
            result.AsT0.Value.Name,
            result.AsT0.Value.Balance,
            result.AsT0.Value.IsDefault)) : BadRequest(result.AsT1);
    }

    [HttpGet("Default")]
    public async Task<ActionResult<GetAccountResponse>> GetDefaultAccount(CancellationToken cancellationToken)
    {
        var result = await _accountService.GetDefaultAccount(cancellationToken);
        return result.IsT0 ? Ok(new GetAccountDto(
            result.AsT0.Value.Id,
            result.AsT0.Value.Name,
            result.AsT0.Value.Balance,
            result.AsT0.Value.IsDefault)) : BadRequest(result.AsT1);
    }

    [HttpGet]
    public async Task<ActionResult<IList<GetAccountResponse>>> GetAccounts(CancellationToken cancellationToken)
    {
        var result = await _accountService.GetAllAccounts(cancellationToken);
        return result.IsT0 ? Ok(result.AsT0.Value.Select(account => new GetAccountDto(
            account.Id,
            account.Name,
            account.Balance,
            account.IsDefault))) : BadRequest(result.AsT1);
    }
}