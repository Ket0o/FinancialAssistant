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

    [HttpPost("[action]")]
    public async Task<ActionResult> CreateAccount([FromBody]string name, CancellationToken cancellationToken)
    {
        var result = await _accountService.AddAccount(name, cancellationToken);

        return result.IsT0 ? Ok(result.AsT0) : BadRequest(result.AsT1);
    }
}