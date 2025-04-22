using AutoMapper;
using clothes_backend.DTO.ORDER;
using clothes_backend.Models;

namespace clothes_backend.AutoMapper
{
    public class OrderProfile:Profile
    {
        public OrderProfile()
        {
            CreateMap<CartItems, OrderDetails>()
                .ForMember(x=>x.id, y=>y.Ignore())
                .ForMember(x=>x.product_variant_id, y=>y.MapFrom(z=>z.product_variant_id))
                .ForMember(x=>x.quantity, y=>y.MapFrom(z=>z.quantity))
                .ForMember(x=>x.price, y=>y.MapFrom(z=>z.product_variants.price));
            CreateMap<OrderValueDTO, OrderDetails>()
               .ForMember(x => x.id, y => y.Ignore())
               .ForMember(x => x.product_variant_id, y => y.MapFrom(z => z.id))
               .ForMember(x => x.quantity, y => y.MapFrom(z => z.purchase_quantity))
               .ForMember(x => x.price, y => y.MapFrom(z => z.price));
            CreateMap<Orders, orderDTO>();
        }
    }
}
