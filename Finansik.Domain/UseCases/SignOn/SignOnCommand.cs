using Finansik.Domain.Authentication;
using MediatR;

namespace Finansik.Domain.UseCases.SignOn;

public record SignOnCommand(string Login, string Password) : IRequest<IIdentity>;