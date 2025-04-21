using AutoMapper;
using Azure.Core;
using clothes_backend.DTO.General;
using clothes_backend.DTO.ORDER;
using clothes_backend.Inteface;
using clothes_backend.Inteface.Order;
using clothes_backend.Models;
using clothes_backend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace clothes_backend.Repository
{
    public class OrderRepository : GenericRepository<Orders>,IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        public OrderRepository(DatabaseContext db, IMapper mapper, IMemoryCache cache) : base(db)
        {
            _mapper = mapper;
            _cache = cache;
        }
        //lay list item tu cart/ co the khong lay het
        public async Task<PayloadDTO<Orders>> add([FromForm]orderItemDTO DTO)
        {
            try
            {               
                //kiem tra user, cartItem co ton tai khong              
                //lay gio hang hien tai cua user            
                ////load cartItem        
                //slban + sl mua > sl ton return false;               
                var cartItem = await _db.cart_items
                      .Include(x => x.product_variants)
                      .Where(x => DTO.cartItem_id.Contains(x.id) && x.carts.user_id == DTO.user_id)
                      .ToListAsync();

                if(!cartItem.Any()) return PayloadDTO<Orders>.Error(Utils.Enum.StatusCode.NotFound);
                if (cartItem.Count != DTO.cartItem_id.Count) return PayloadDTO<Orders>.Error(Utils.Enum.StatusCode.Isvalid);
                //kiem tra so luong
                              
                var orderDetail_ATM = _mapper.Map<List<OrderDetails>>(cartItem);

                var order = new Orders()
                {
                    user_id = DTO.user_id,
                    status = "PENDING",
                    phone = DTO.phone,
                    payment_type = DTO.payment_type,
                    address = DTO.address,
                    order_details = orderDetail_ATM,
                    note = DTO.note,
                    total = orderDetail_ATM.Sum(x => x.price*x.quantity),                 
                };
                _db.orders.Add(order);
                _db.cart_items.RemoveRange(cartItem);//xoa cartItem da order
                await _db.SaveChangesAsync();
                return PayloadDTO<Orders>.OK(order);
            }
            catch
            {
                return PayloadDTO<Orders>.Error(Utils.Enum.StatusCode.Isvalid);
            }         
        }                   
        public Task<PayloadDTO<Orders>> getAll()
        {
            throw new NotImplementedException();
        }
        public Task<PayloadDTO<Orders>> getId()
        {
            throw new NotImplementedException();
        }
    }
}
