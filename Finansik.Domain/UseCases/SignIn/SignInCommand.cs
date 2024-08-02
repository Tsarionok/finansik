using Finansik.Domain.Authentication;
using Finansik.Domain.Monitoring;
using MediatR;

namespace Finansik.Domain.UseCases.SignIn;

public record SignInCommand(string Login, string Password) : 
    DefaultMonitoredRequest("user.sign-in"), IRequest<(IIdentity identity, string token)>;
