using AutoMapper;
using clothes_backend.DTO.CART;
using clothes_backend.DTO.General;
using clothes_backend.Inteface.Cart;
using clothes_backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Security;

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
        public async Task<PayloadDTO<CartItemDTO>> getCart(int user_id)
        {
            try
            {
                var carts = await _db.carts.FindAsync(user_id);
                if (carts == null) return PayloadDTO<CartItemDTO>.Error(Utils.Enum.StatusCode.NotFound);
                await _db.Entry(carts).Collection(x => x.cartItems).LoadAsync();
                //automapper
                var cartItem = _mapper.Map<CartItemDTO>(carts.cartItems);
                return PayloadDTO<CartItemDTO>.OK(cartItem);
            }
            catch
            {
                return PayloadDTO<CartItemDTO>.Error(Utils.Enum.StatusCode.Isvalid);
            }
          
        }

        public Task<PayloadDTO<CartItemDTO>> removeCartItem(int id)
        {
            throw new NotImplementedException();
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
        public object getIsUser()
        {
            
            var httpContext = _context.HttpContext;
            if (httpContext != null && httpContext.Items.ContainsKey("IsUser"))
            {
                var value =  httpContext.Items["IsUser"];
                Console.WriteLine($"User ID: {value}");
                return value;
            }
            else
            {
                
                Console.WriteLine("User ID not found.");
                return null;
            }
           
        }

    }
}
