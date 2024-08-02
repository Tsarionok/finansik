using Finansik.Domain.Monitoring;
using MediatR;

namespace Finansik.Domain.UseCases.SignOut;

public record SignOutCommand() : DefaultMonitoredRequest("user.sign-out"), IRequest;