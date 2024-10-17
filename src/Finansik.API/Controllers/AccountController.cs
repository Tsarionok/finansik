using Finansik.API.Authentication;
using Finansik.API.Models;
using Finansik.Domain.UseCases.SignIn;
using Finansik.Domain.UseCases.SignOn;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Finansik.API.Controllers;

[ApiController]
[Route("account")]
public sealed class AccountController(IMediator mediator) : ControllerBase
{
    [HttpPost("signon")]
    public async Task<IActionResult> SignOn(
        [FromBody] SignOn request,
        CancellationToken cancellationToken
        )
    {
        var identity = await mediator.Send(
            new SignOnCommand(request.Login, request.Password), cancellationToken);
        
        return Ok(identity);
    }
    
    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(
        [FromBody] SignIn request,
        [FromServices] IAuthTokenStorage authTokenStorage,
        CancellationToken cancellationToken
    )
    {
        var (identity, token) = await mediator.Send(
            new SignInCommand(request.Login, request.Password), cancellationToken);
        authTokenStorage.Store(HttpContext, token);
        
        return Ok(identity);
    }
}