using clothes_backend.DTO.General;
using clothes_backend.DTO.ORDER;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Interfaces
{
    public interface IOrderService
    {
        Task<Result<Orders>> add([FromForm]orderItemDTO DTO);
        Task<Result<List<orderDetailDTO>>> getId(int id);
        Task<Result<List<orderDTO>>> getAll();
    }
}
