using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAssistant.Web.Controllers.Accounts;

[ApiController]
[Authorize]
[Route("[controller]")]
public class AccountsController : ControllerBase
{
    
}