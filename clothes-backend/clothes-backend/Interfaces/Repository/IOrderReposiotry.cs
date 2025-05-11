using clothes_backend.DTO.General;
using clothes_backend.DTO.ORDER;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Interfaces.Repository
{
    public interface IOrderReposiotry
    {
        Task<Result<Orders>> add([FromForm] orderItemDTO DTO, int user_id);
        Task<Result<List<OrderDetails>>> getId(int id, int user_id);
        Task<Result<List<Orders>>> getAll(int user_id);
    }
}
