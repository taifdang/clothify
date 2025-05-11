using AutoMapper;
using AutoMapper.QueryableExtensions;
using clothes_backend.Data;
using clothes_backend.DTO.CART;
using clothes_backend.DTO.General;
using clothes_backend.Interfaces;
using clothes_backend.Interfaces.Service;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
namespace clothes_backend.Repository
{
    public class CartRepositoryOld : BaseRepository<Carts>,ICartServiceOld,ICartUtils
    {
        private readonly IMapper _mapper;     
        private readonly IUserContextService _authService;
        public CartRepositoryOld(DatabaseContext db,IMapper mapper, IUserContextService authService) : base(db)
        {          
            _mapper = mapper;        
            _authService = authService;
        }
        public async Task<Result<CartItemDTO>> addCartItem([FromForm] cartAddDTO DTO)
        {
            try
            {
                var user_id = _authService.convertToInt(_authService.getValueAuth());
                if (user_id == 0) return Result<CartItemDTO>.Failure(Utils.Enum.StatusCode.Unauthorized);

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
                return Result<CartItemDTO>.Success(data);
            }
            catch
            {
                return Result<CartItemDTO>.Failure(Utils.Enum.StatusCode.None);
            }              
        }            
        public async Task<Result<List<CartItemDTO>>> getCart()
        {
            try
            {
                var user_id =_authService.convertToInt(_authService.getValueAuth());
                if (user_id == 0) return Result<List<CartItemDTO>>.Failure(Utils.Enum.StatusCode.Unauthorized);             
                var carts = await _db.carts.FirstOrDefaultAsync(x => x.user_id == user_id);
                if (carts == null) return Result<List<CartItemDTO>>.Failure(Utils.Enum.StatusCode.NotFound);
                var listCartItem = await _db.cart_items.Where(x => x.cart_id == carts.id).ProjectTo<CartItemDTO>(_mapper.ConfigurationProvider).ToListAsync();              
                return Result<List<CartItemDTO>>.Success(listCartItem);
            }
            catch
            {
                return Result<List<CartItemDTO>>.Failure(Utils.Enum.StatusCode.Isvalid);
            }
        }
        //Concurrenttly conflict 1
        public async Task<Result<CartItems>> removeCartItem([FromForm] cartDeleteDTO DTO)
        {
            try
            {
                var user_id = _authService.convertToInt(_authService.getValueAuth());
                if (user_id == 0) return Result<CartItems>.Failure(Utils.Enum.StatusCode.Unauthorized);
                //kiem tra gio hang cua user            
                var cartItem = await checkCartItems(DTO.id, user_id);
                if (cartItem == null) return Result<CartItems>.Failure(Utils.Enum.StatusCode.NotFound);
                if (!cartItem.row_version.SequenceEqual(DTO.row_version))//neu row_version khac nhau thi loi
                {
                    return Result<CartItems>.Failure(Utils.Enum.StatusCode.Conflict);
                }
                _db.cart_items.Remove(cartItem); //neu ton tai thi xoa                
                await _db.SaveChangesAsync();
                return Result<CartItems>.Success(null!);               
            }
            catch
            {
                return Result<CartItems>.Failure(Utils.Enum.StatusCode.Isvalid);
            }
        }
        //kiem tra cart cua user co cartItem do thi xoa
        public async Task<CartItems?> checkCartItems(int? cartItem_id, int user) => await _db.cart_items.Include(x => x.carts).FirstOrDefaultAsync(x => x.carts.user_id == user && x.id == cartItem_id) ?? null;    
        //Concurrenttly conflict 2  
        public async Task<Result<CartItemDTO>> updateCartItem([FromForm] CartItemDTO DTO)
        {
            try
            {
                var user_id = _authService.convertToInt(_authService.getValueAuth());
                if (user_id == 0) return Result<CartItemDTO>.Failure(Utils.Enum.StatusCode.Unauthorized);
                var cartItem = await checkCartItems(DTO.id, user_id);
                if (cartItem == null) return Result<CartItemDTO>.Failure(Utils.Enum.StatusCode.NotFound);
                cartItem.product_variant_id = DTO.product_variant_id;
                cartItem.quantity = DTO.quantity;            
                _db.Entry(cartItem).Property(p => p.row_version).OriginalValue = DTO.row_version;                         
                try
                {
                    await _db.SaveChangesAsync();
                    return Result<CartItemDTO>.Success(null!);
                }
                catch(DbUpdateConcurrencyException ex)
                {                   
                    //neu that bai => reload rowversion
                    ex.Entries.Single().Reload();
                    return Result<CartItemDTO>.Failure(Utils.Enum.StatusCode.Conflict);
                }
            }
            catch
            {
                return Result<CartItemDTO>.Failure(Utils.Enum.StatusCode.Isvalid);
            }       
        }
    }
}
