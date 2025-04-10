using AutoMapper;
using clothes_backend.DTO;
using clothes_backend.Models;

namespace clothes_backend.AutoMapper
{
    public class ImageProfile:Profile
    {
        public ImageProfile()
        {
            CreateMap<imageDTO, ProductOptionImages>();
        }
    }
}
