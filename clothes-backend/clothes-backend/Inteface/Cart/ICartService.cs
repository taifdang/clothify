using clothes_backend.DTO.CART;
using clothes_backend.DTO.General;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Inteface.Cart
{
    public interface ICartService
    {
        Task<PayloadDTO<CartItemDTO>> addCartItem([FromForm]CartItemDTO DTO);
        Task<PayloadDTO<CartItemDTO>> removeCartItem(int id);
        Task<PayloadDTO<CartItemDTO>> updateCartItem([FromForm]CartItemDTO DTO);
        Task<PayloadDTO<CartItemDTO>> getCart(int user_id);
        //void mergeCart();
    }
}
