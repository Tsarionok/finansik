using Finansik.Domain.Exceptions;
using Finansik.Domain.Models;
using MediatR;

namespace Finansik.Domain.UseCases.GetCategories;

internal class GetCategoriesByGroupIdUseCase(IGetCategoriesByGroupIdStorage storage) : 
    IRequestHandler<GetCategoriesByGroupIdQuery, IEnumerable<Category>>
{
    public async Task<IEnumerable<Category>> Handle(GetCategoriesByGroupIdQuery query, CancellationToken cancellationToken)
    {
        var groupExists = await storage.IsGroupExists(query.GroupId, cancellationToken);
        
        if (!groupExists)
        {
            throw new GroupNotFoundException(query.GroupId);
        }

        return await storage.GetCategoriesByGroupId(query.GroupId, cancellationToken);
    }
}