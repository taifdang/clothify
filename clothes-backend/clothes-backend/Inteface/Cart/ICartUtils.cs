using clothes_backend.Models;

namespace clothes_backend.Inteface.Cart
{
    public interface ICartUtils
    {
        Task<CartItems?> checkCartItems(int? cartItem_id,int user);
    }
}
