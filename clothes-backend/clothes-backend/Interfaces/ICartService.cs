using clothes_backend.DTO.CART;
using clothes_backend.DTO.General;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Interfaces
{
    public interface ICartService
    {
        Task<PayloadDTO<CartItemDTO>> addCartItem([FromForm] cartAddDTO DTO);
        Task<PayloadDTO<CartItems>> removeCartItem([FromForm] cartDeleteDTO DTO);
        Task<PayloadDTO<CartItemDTO>> updateCartItem([FromForm]CartItemDTO DTO);
        Task<PayloadDTO<List<CartItemDTO>>> getCart();
        //void mergeCart();
    }
}
