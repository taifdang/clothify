using clothes_backend.DTO.General;
using clothes_backend.DTO.ORDER;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Inteface.Order
{
    public interface IOrderService
    {
        Task<PayloadDTO<Orders>> add([FromForm]orderItemDTO DTO);
        Task<PayloadDTO<List<orderDTO>>> getId(int user, int id);
        Task<PayloadDTO<List<orderDTO>>> getAll(int user);
    }
}
