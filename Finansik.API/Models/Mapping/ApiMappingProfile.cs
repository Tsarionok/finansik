using AutoMapper;

namespace Finansik.API.Models.Mapping;

internal class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        CreateMap<Domain.Models.Category, Category>();
        CreateMap<Domain.Models.Group, Group>();
    }
}