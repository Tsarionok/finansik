using Finansik.API.Authentication;
using Finansik.API.Models;
using Finansik.Domain.UseCases.SignIn;
using Finansik.Domain.UseCases.SignOn;
using Microsoft.AspNetCore.Mvc;

namespace Finansik.API.Controllers;

[ApiController]
[Route("account")]
public class AccountController : ControllerBase
{
    [HttpPost("signon")]
    public async Task<IActionResult> SignOn(
        [FromBody] SignOn request,
        [FromServices] ISignOnUseCase useCase,
        CancellationToken cancellationToken
        )
    {
        var identity = await useCase.Execute(new SignOnCommand(request.Login, request.Password), cancellationToken);
        return Ok(identity);
    }
    
    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(
        [FromBody] SignIn request,
        [FromServices] ISignInUseCase useCase,
        [FromServices] IAuthTokenStorage authTokenStorage,
        CancellationToken cancellationToken
    )
    {
        var (identity, token) = await useCase.Execute(
            new SignInCommand(request.Login, request.Password), cancellationToken);
        
        authTokenStorage.Store(HttpContext, token);
        return Ok(identity);
    }
}