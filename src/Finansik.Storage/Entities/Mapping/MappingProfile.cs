using AutoMapper;
using Finansik.Domain.Models;

namespace Finansik.Storage.Entities.Mapping;

internal sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, Domain.Models.Category>();
        CreateMap<User, RecognisedUser>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
        CreateMap<Session, Domain.Models.Session>();
    }
}