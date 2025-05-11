using AutoMapper;
using clothes_backend.DTO.PRODUCT_DTO;
using clothes_backend.DTO.Test;
using clothes_backend.Models;

namespace clothes_backend.Mappings
{
    public class TestProfile: Profile
    {
        public TestProfile()
        {
            CreateMap<Products, ListPDTO>();
            CreateMap<Products, productListDTO>();
        }
    }
}
