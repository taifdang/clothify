using AutoMapper;
using Infrastructure.Enitites;
using Shared.Models.ProductImage;

namespace Application.Common.Mapping;

public class ProductImageProfile : Profile
{
    public ProductImageProfile()
    {
        CreateMap<ProductImage, ProductImageDTO>().ReverseMap();
    }
}
