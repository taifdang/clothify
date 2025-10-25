
using Application.Common.Models.Product;
using AutoMapper;
using Infrastructure.Enitites;

namespace Application.Common.Mapping;

public class ProductProfile : Profile
{
   public ProductProfile()
   {
        CreateMap<Product, ProductDTO>().ReverseMap();

        CreateMap<AddProductRequest, Product>();

        CreateMap<UpdateProductRequest, Product>();
   }
}
