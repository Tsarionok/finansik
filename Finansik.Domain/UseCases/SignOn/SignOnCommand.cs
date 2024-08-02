using Finansik.Domain.Authentication;
using Finansik.Domain.Monitoring;
using MediatR;

namespace Finansik.Domain.UseCases.SignOn;

public record SignOnCommand(string Login, string Password) : 
    DefaultMonitoredRequest("user.sign-on"), IRequest<IIdentity>;