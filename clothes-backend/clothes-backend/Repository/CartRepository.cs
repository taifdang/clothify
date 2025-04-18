using AutoMapper;
using AutoMapper.QueryableExtensions;
using clothes_backend.DTO.CART;
using clothes_backend.DTO.General;
using clothes_backend.Inteface.Cart;
using clothes_backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Security;
using System.Security.Cryptography.Xml;

namespace clothes_backend.Repository
{
    public class CartRepository : GenericRepository<Carts>,ICartService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _context;
        public CartRepository(DatabaseContext db,IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(db)
        {          
            _mapper = mapper;
            _context = httpContextAccessor;
        }
        public async Task<PayloadDTO<CartItemDTO>> addCartItem([FromForm] CartItemDTO DTO)
        {
            try
            {
                getAuth(out int user_id);               
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
                getAuth(out int user_id);
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
        public Task<PayloadDTO<CartItemDTO>> updateCartItem([FromForm] CartItemDTO DTO)
        {
            throw new NotImplementedException();
        }
        public void getAuth(out int user)
        {
           
            if (_context.HttpContext.Items.TryGetValue("IsUser", out var value))
            {
                user = Convert.ToInt32(value?.ToString());
                Console.WriteLine($"User ID: {value}");
            }
            else
            {
                user = 0;
                Console.WriteLine("User ID not found.");
            }
           
        }      
        public async Task<PayloadDTO<CartItems>> removeCartItem(int id)
        {
            try
            {
                 getAuth(out int user);
                if (user == 0) return PayloadDTO<CartItems>.Error(Utils.Enum.StatusCode.Unauthorized);
                //kiem tra gio hang cua user
                var cartItem = await _db.cart_items.Include(x => x.carts).FirstOrDefaultAsync(x => x.carts.user_id == user && x.id == id);
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
    }
}
