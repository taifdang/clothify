using AutoMapper;
using clothes_backend.DTO;
using clothes_backend.Models;

namespace clothes_backend.AutoMapper
{
    public class ProductProfile:Profile
    {
       public ProductProfile()
       {
            CreateMap<productsDTO,Products >()
                .ForMember(pro=>pro.id,opt=>opt.Ignore());
       }
    }
}
