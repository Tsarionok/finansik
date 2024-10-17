using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.CreateScheduledOperaton;

public interface IScheduleOperation
{
    Task<ScheduledOperation> ScheduleOperation(ScheduleOperationCommand command,
        CancellationToken cancellationToken);
}