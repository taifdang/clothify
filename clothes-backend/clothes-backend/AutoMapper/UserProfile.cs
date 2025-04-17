using AutoMapper;
using clothes_backend.DTO.USER;
using clothes_backend.Models;

namespace clothes_backend.AutoMapper
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<Users, userInfoDTO>()
                .ForMember(x=>x.id,target=>target.MapFrom(y=>y.id));
        }
    }
}
