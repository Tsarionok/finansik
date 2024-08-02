using Finansik.Domain.Authentication;
using MediatR;

namespace Finansik.Domain.UseCases.SignIn;

public record SignInCommand(string Login, string Password) : IRequest<(IIdentity identity, string token)>;
