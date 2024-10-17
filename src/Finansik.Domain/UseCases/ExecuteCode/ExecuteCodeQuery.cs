using Finansik.Domain.Monitoring;
using MediatR;

namespace Finansik.Domain.UseCases.ExecuteCode;

public record ExecuteCodeQuery(string Code, string StartMethod, dynamic Argument) 
    : DefaultMonitoredRequest("code.executed"), IRequest<ExecuteCodeQueryResult>;