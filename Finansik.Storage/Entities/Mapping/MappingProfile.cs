using AutoMapper;

namespace Finansik.Storage.Entities.Mapping;

internal class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, Domain.Models.Category>();
    }
}