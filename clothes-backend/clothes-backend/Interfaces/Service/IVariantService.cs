using clothes_backend.DTO.General;
using clothes_backend.DTO.VARIANT;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Interfaces.Service
{
    public interface IVariantService
    {
        Task<Result<ProductVariants>> GetIdVariant([FromForm] ProductVariantDTO DTO);
        Task<Result<ProductVariants>> AddVariant([FromForm] ProductVariantDTO DTO);
        Task<Result<ProductVariants>> DeleteVariant(int id);
    }
}
