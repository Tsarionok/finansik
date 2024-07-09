using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.GetGroups;

public interface IGetGroupsUseCase : IUseCase<IEnumerable<Group>>;