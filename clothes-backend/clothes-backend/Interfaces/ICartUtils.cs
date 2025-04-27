using clothes_backend.Models;

namespace clothes_backend.Interfaces
{
    public interface ICartUtils
    {
        Task<CartItems?> checkCartItems(int? cartItem_id,int user);
    }
}
