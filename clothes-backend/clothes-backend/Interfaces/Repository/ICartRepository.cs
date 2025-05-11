using clothes_backend.DTO.CART;
using clothes_backend.DTO.General;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Interfaces.Repository
{
    public interface ICartRepository:IBaseRepository<Carts>,ICartUtils
    {
        Task<Result<CartItems>> addCartItem([FromForm] cartAddDTO DTO,int user_id);
        Task<Result<CartItems>> removeCartItem([FromForm] cartDeleteDTO DTO,int user_id);
        Task<Result<CartItemDTO>> updateCartItem([FromForm] CartItemDTO DTO,int user_id);
        Task<Result<List<CartItems>>> getCart(int user_id);      
    }
}
