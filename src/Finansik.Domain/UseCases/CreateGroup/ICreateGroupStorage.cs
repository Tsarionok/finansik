using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.CreateGroup;

public interface ICreateGroupStorage
{
    Task<Group> CreateGroup(string name, Guid creator, string? icon, CancellationToken cancellationToken);
}