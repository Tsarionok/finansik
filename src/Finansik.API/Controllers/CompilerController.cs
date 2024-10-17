using Finansik.API.Models;
using Finansik.Domain.UseCases.ExecuteCode;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Finansik.API.Controllers;

[ApiController]
[Route("compiler")]
public class CompilerController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Compile(
        [FromBody] CodeInfo codeInfo,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new ExecuteCodeQuery(codeInfo.Code, codeInfo.StartMethod, codeInfo.Argument), cancellationToken);

        return Ok(result.Output);
    }
}