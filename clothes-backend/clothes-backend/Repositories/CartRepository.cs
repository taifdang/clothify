using AutoMapper.QueryableExtensions;
using clothes_backend.Data;
using clothes_backend.DTO.CART;
using clothes_backend.DTO.General;
using clothes_backend.Interfaces.Repository;
using clothes_backend.Models;
using clothes_backend.Repository;
using clothes_backend.Utils.Enum;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace clothes_backend.Repositories
{
    public class CartRepository : BaseRepository<Carts>, ICartRepository
    {
        public CartRepository(DatabaseContext db) : base(db)
        {
        }

        public async Task<Result<CartItems>> addCartItem([FromForm] cartAddDTO DTO,int user_id)
        {
            try
            {
                var cart = await _db.carts.Include(x => x.cartItems).FirstOrDefaultAsync(x => x.user_id == user_id);
                if (cart is null)
                {
                    _db.carts.Add(new Carts { user_id = user_id });
                    await _db.SaveChangesAsync();
                }
                //add cartItem
                var cartItem = await _db.cart_items.Where(x => x.cart_id == cart.id && x.product_variant_id == DTO.product_variant_id).FirstOrDefaultAsync();
                if (cartItem is not null)
                {
                    cartItem.quantity += DTO.quantity;
                }
                else
                {
                    _db.cart_items.Add(
                        new CartItems
                        {
                            cart_id = cart.id,
                            product_variant_id = DTO.product_variant_id,
                            quantity = DTO.quantity
                        });
                }
                await _db.SaveChangesAsync();
                //
                return Result<CartItems>.Success(cartItem);
            }
            catch{
                Result<CartItems>.Failure(StatusCode.Isvalid);
            }
            throw new NotImplementedException();
        }

        public async Task<CartItems?> checkCartItems(int? cartItem_id, int user) => await _db.cart_items.Include(x => x.carts).FirstOrDefaultAsync(x => x.carts.user_id == user && x.id == cartItem_id) ?? null;


        public async Task<Result<List<CartItems>>> getCart(int user_id)
        {
            try
            {        
                var carts = await _db.carts.FirstOrDefaultAsync(x => x.user_id == user_id);
                if (carts == null) return Result<List<CartItems>>.Failure(Utils.Enum.StatusCode.NotFound);
                //var listCartItem = await _db.cart_items.Where(x => x.cart_id == carts.id).ProjectTo<CartItemDTO>(_mapper.ConfigurationProvider).ToListAsync();
                var listCartItem = await _db.cart_items.Where(x => x.cart_id == carts.id).ToListAsync();
                return Result<List<CartItems>>.Success(listCartItem);
            }
            catch
            {
                return Result<List<CartItems>>.Failure(Utils.Enum.StatusCode.Isvalid);
            }
        }

        public async Task<Result<CartItems>> removeCartItem([FromForm] cartDeleteDTO DTO,int user_id)
        {
            try
            {             
                //kiem tra gio hang cua user            
                var cartItem = await checkCartItems(DTO.id, user_id);
                if (cartItem == null) return Result<CartItems>.Failure(Utils.Enum.StatusCode.NotFound);
                if (!cartItem.row_version.SequenceEqual(DTO.row_version))//neu row_version khac nhau thi loi
                {
                    return Result<CartItems>.Failure(Utils.Enum.StatusCode.Conflict);
                }
                _db.cart_items.Remove(cartItem); //neu ton tai thi xoa                
                await _db.SaveChangesAsync();
                return Result<CartItems>.Success();
            }
            catch
            {
                return Result<CartItems>.Failure(Utils.Enum.StatusCode.Isvalid);
            }
        }

        public async Task<Result<CartItemDTO>> updateCartItem([FromForm] CartItemDTO DTO,int user_id)
        {
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
            catch (DbUpdateConcurrencyException ex)
            {
                //neu that bai => reload rowversion
                ex.Entries.Single().Reload();
                return Result<CartItemDTO>.Failure(Utils.Enum.StatusCode.Conflict);
            }
        }
    }
}
