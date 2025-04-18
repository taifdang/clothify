using AutoMapper;
using clothes_backend.DTO.CART;
using clothes_backend.Models;

namespace clothes_backend.AutoMapper
{
    public class CartProfile:Profile
    {
        public CartProfile()
        {
            CreateMap<CartItems, CartItemDTO>()              
                .ForMember(x=>x.id,target => target.MapFrom(y=>y.id))
                .ForMember(x=>x.row_version,target=>target.MapFrom(y=>y.row_version));

            CreateMap<CartItemDTO, CartItems>()
              .ForMember(x => x.id, target => target.MapFrom(y => y.id))
              .ForMember(x => x.row_version, target => target.MapFrom(y => y.row_version));

        }
    }
}
