using Finansik.Domain.Monitoring;
using MediatR;

namespace Finansik.Domain.UseCases.SignOut;

public sealed record SignOutCommand() : DefaultMonitoredRequest("user.sign-out"), IRequest;