using clothes_backend.DTO.General;
using clothes_backend.DTO.ORDER;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Inteface.Order
{
    public interface IOrderService
    {
        Task<PayloadDTO<Orders>> add([FromForm]orderItemDTO DTO);
        Task<PayloadDTO<Orders>> remove();       
        Task<PayloadDTO<Orders>> getId();
        Task<PayloadDTO<Orders>> getAll();
    }
}
