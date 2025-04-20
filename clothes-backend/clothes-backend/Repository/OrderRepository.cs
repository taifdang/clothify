using Azure.Core;
using clothes_backend.DTO.General;
using clothes_backend.DTO.ORDER;
using clothes_backend.Inteface.Order;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace clothes_backend.Repository
{
    public class OrderRepository : GenericRepository<Orders>,IOrderService
    {
        public OrderRepository(DatabaseContext db) : base(db)
        {
        }
        //lay list item tu cart/ co the khong lay het
        public async Task<PayloadDTO<Orders>> add([FromForm]orderItemDTO DTO)
        {
            try
            {
                //kiem tra user, cartItem co ton tai khong              
                //lay gio hang hien tai cua user            
                ////load cartItem                          
                var cartItem = await _db.cart_items
                      .Include(x => x.product_variants)
                      .Where(x => DTO.cartItem_id.Contains(x.id) && x.carts.user_id == DTO.user_id)
                      .ToListAsync();
                if(!cartItem.Any()) return PayloadDTO<Orders>.Error(Utils.Enum.StatusCode.NotFound);              
                var order = new Orders()
                {
                    user_id = DTO.user_id,
                    status = "PENDING",
                    phone = DTO.phone,
                    payment_type = DTO.payment_type,
                    address = DTO.address,
                    order_details = new List<OrderDetails>(),
                    note = DTO.note,
                    create_at = DateTime.Now
                };
                foreach (var item in cartItem) //temporary key: khoa tam
                {
                    var price = item.product_variants.price;
                    var quantity = item.quantity;
                    var orderDetail = new OrderDetails
                    {
                        product_variant_id = item.product_variant_id,
                        price = price,
                        quantity = quantity,
                    };
                    order.total += (price * quantity);
                    order.order_details.Add(orderDetail);
                }
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

        public Task<PayloadDTO<Orders>> remove()
        {
            throw new NotImplementedException();
        }
    }
}
