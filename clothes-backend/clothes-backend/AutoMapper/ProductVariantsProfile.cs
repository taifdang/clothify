using AutoMapper;
using clothes_backend.DTO;
using clothes_backend.Models;

namespace clothes_backend.AutoMapper
{
    public class ProductVariantsProfile:Profile
    {
        public ProductVariantsProfile()
        {
            CreateMap<productVariantsDTO, ProductVariants>()
                .ForMember(opt => opt.id, x => x.Ignore())              
                .ForMember(opt => opt.percent, x => x.MapFrom(src =>
                     (src.old_price > 0)
                        ? (decimal)Math.Ceiling((src.old_price - src.price) / src.old_price * 100)
                        : 0));

        }
    }  
}
