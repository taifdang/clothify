using AutoMapper;
using AutoMapper.QueryableExtensions;
using clothes_backend.DTO.CART;
using clothes_backend.DTO.General;
using clothes_backend.Inteface.Cart;
using clothes_backend.Inteface.User;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace clothes_backend.Repository
{
    public class CartRepository : GenericRepository<Carts>,ICartService,ICartUtils
    {
        private readonly IMapper _mapper;     
        private readonly IAuthService _authService;
        public CartRepository(DatabaseContext db,IMapper mapper, IAuthService authService) : base(db)
        {          
            _mapper = mapper;        
            _authService = authService;
        }
        public async Task<PayloadDTO<CartItemDTO>> addCartItem([FromForm] CartItemDTO DTO)
        {
            try
            {
                var user_id = _authService.convertToInt(_authService.getValueAuth());
                if (user_id == 0) return PayloadDTO<CartItemDTO>.Error(Utils.Enum.StatusCode.Unauthorized);

                var carts = _db.carts.Include(x => x.cartItems).FirstOrDefault(x => x.user_id == user_id);
                if (carts is null)
                {
                    _db.carts.Add(new Carts { user_id = user_id });
                    await _db.SaveChangesAsync();
                }             
                var cartItem = await _db.cart_items.Where(x=>x.cart_id == carts.id && x.product_variant_id == DTO.product_variant_id).FirstOrDefaultAsync();
                if (cartItem is not null)
                {
                    cartItem.quantity += DTO.quantity;
                }
                else
                {
                    _db.cart_items.Add(
                        new CartItems 
                        {                      
                            cart_id = carts.id, 
                            product_variant_id = DTO.product_variant_id, 
                            quantity = DTO.quantity 
                        });
                }
                await _db.SaveChangesAsync();
                //automapper
                var data = _mapper.Map<CartItemDTO>(cartItem);
                return PayloadDTO<CartItemDTO>.OK(data);
            }
            catch
            {
                return PayloadDTO<CartItemDTO>.Error(Utils.Enum.StatusCode.None);
            }              
        }      
        //test
        public async Task<PayloadDTO<List<CartItemDTO>>> getCart()
        {
            try
            {
                var user_id =_authService.convertToInt(_authService.getValueAuth());
                if (user_id == 0) return PayloadDTO<List<CartItemDTO>>.Error(Utils.Enum.StatusCode.Unauthorized);
                //
                var carts = await _db.carts.FirstOrDefaultAsync(x => x.user_id == user_id);
                if (carts == null) return PayloadDTO<List<CartItemDTO>>.Error(Utils.Enum.StatusCode.NotFound);
                var listCartItem = await _db.cart_items.Where(x => x.cart_id == carts.id).ProjectTo<CartItemDTO>(_mapper.ConfigurationProvider).ToListAsync();
                return PayloadDTO<List<CartItemDTO>>.OK(listCartItem);
            }
            catch
            {
                return PayloadDTO<List<CartItemDTO>>.Error(Utils.Enum.StatusCode.Isvalid);
            }
        }         
        public async Task<PayloadDTO<CartItems>> removeCartItem(int id)
        {
            try
            {
                var user_id = _authService.convertToInt(_authService.getValueAuth());
                if (user_id == 0) return PayloadDTO<CartItems>.Error(Utils.Enum.StatusCode.Unauthorized);
                //kiem tra gio hang cua user            
                var cartItem = await checkCartItems(id, user_id);
                if (cartItem == null) return PayloadDTO<CartItems>.Error(Utils.Enum.StatusCode.NotFound);
                _db.cart_items.Remove(cartItem); //neu ton tai thi xoa
                await _db.SaveChangesAsync();
                return PayloadDTO<CartItems>.OK(cartItem);
            }
            catch
            {
                return PayloadDTO<CartItems>.Error(Utils.Enum.StatusCode.Isvalid);
            }
        }
        public async Task<CartItems?> checkCartItems(int? cartItem_id, int user) => await _db.cart_items.Include(x => x.carts).FirstOrDefaultAsync(x => x.carts.user_id == user && x.id == cartItem_id) ?? null;
      
        public async Task<PayloadDTO<CartItemDTO>> updateCartItem([FromForm] CartItemDTO DTO)
        {
            try
            {
                var user_id = _authService.convertToInt(_authService.getValueAuth());
                if (user_id == 0) return PayloadDTO<CartItemDTO>.Error(Utils.Enum.StatusCode.Unauthorized);
                var cartItem = await checkCartItems(DTO.id, user_id);
                if (cartItem == null) return PayloadDTO<CartItemDTO>.Error(Utils.Enum.StatusCode.NotFound);
                //update
                cartItem.product_variant_id = DTO.product_variant_id;
                cartItem.quantity = DTO.quantity;
                await _db.SaveChangesAsync();

                return PayloadDTO<CartItemDTO>.OK(null!);
            }
            catch
            {
                return PayloadDTO<CartItemDTO>.Error(Utils.Enum.StatusCode.Isvalid);
            }       
        }
    }
}
