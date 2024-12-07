using FinancialAssistant.Authentication;
using FinancialAssistant.DataTransfer.Password;
using FinancialAssistant.DataTransfer.User;
using FinancialAssistant.EmailService;
using FinancialAssistant.Web.Controllers.Authorization.Requests;
using FinancialAssistant.Web.Controllers.Authorization.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace FinancialAssistant.Web.Controllers.Authorization;

[ApiController]
[Route("[controller]")]
public class AuthorizationController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;

    public AuthorizationController(IUserService userService, 
        IEmailService emailService)
    {
        _userService = userService;
        _emailService = emailService;
    }

    /// <summary>
    /// Зарегистрироваться.
    /// </summary>
    /// <param name="request">Запрос на регистрацию.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [HttpPost("[action]")]
    public async Task<ActionResult> Register(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _userService.CreateUser(new CreateUserDto(request.UserName, request.Email, request.Password),
            cancellationToken);

        return result.IsT0 ? Ok(result.AsT0.Value) : BadRequest(result.AsT1.Value);
    }

    /// <summary>
    /// Войти.
    /// </summary>
    /// <param name="request">Запрос на вход.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Токены.</returns>
    [HttpPost("[action]")]
    public async Task<ActionResult<LoginUserResponse>> Login(LoginUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _userService.AuthenticationUser(new AuthorizationUserDto(request.Email, request.Password),
            cancellationToken);

        return result.IsT0
            ? Ok(new LoginUserResponse(result.AsT0.AccessToken, result.AsT0.RefreshToken))
            : BadRequest(result.AsT1);
    }

    /// <summary>
    /// Сменить пароль.
    /// </summary>
    /// <param name="email">Почта.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [HttpPost("[action]")]
    public async Task<ActionResult> ForgotPassword([FromBody]string email, CancellationToken cancellationToken)
    {
        var result = await _userService.ForgotPassword(email, cancellationToken);

        if (result.IsT1)
            return BadRequest($"{result.AsT1.Value}");
        
        await _emailService.SendEmail(new PasswordResetEmailDto(email, result.AsT0.Value), cancellationToken);
        return Ok("Код отправлен на почту.");
    }

    /// <summary>
    /// Подтвердить смену пароля кодом с почты.
    /// </summary>
    /// <param name="request">Запрос на смену пароля с подтверждением почты.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [HttpPost("[action]")]
    public async Task<ActionResult> EmailAccess(EmailAccessRequest request, CancellationToken cancellationToken)
    {
        var result = await _userService.ChangeUserPassword(new ChangePasswordDto(request.Email, request.NewPassword,
            request.Code), cancellationToken);

        return result.IsT0 ? Ok(result.AsT0.Value) : BadRequest(result.AsT1.Value);
    }
}