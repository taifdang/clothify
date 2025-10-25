using Application.Common.Models.ProductVariant;
using AutoMapper;
using Infrastructure.Enitites;
using Shared.Models.ProductVariant;

namespace Application.Common.Mapping;

public class ProductVariantProfile : Profile
{
    public ProductVariantProfile()
    {
        CreateMap<ProductVariant, ProductVariantDTO>().ReverseMap();

        CreateMap<UpdateProductVariantRequest, ProductVariant>();
    }
}
