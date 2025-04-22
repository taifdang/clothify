using AutoMapper;
using clothes_backend.DTO.IMAGE;
using clothes_backend.Models;

namespace clothes_backend.AutoMapper
{
    public class ImageProfile:Profile
    {
        public ImageProfile()
        {
            CreateMap<imageUploadDTO, ProductOptionImages>();
        }
    }
}
