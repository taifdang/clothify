using AutoMapper;
using Infrastructure.Enitites;
using Shared.Models.User;

namespace Application.Common.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserUpdateRequest, User>().ForMember(x => x.Id, y => y.Ignore());
    }
}
