using AutoMapper;
using clothes_backend.DTO.CART;
using clothes_backend.DTO.General;
using clothes_backend.Interfaces.Repository;
using clothes_backend.Interfaces.Service;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repository;
        private readonly IUserContextService _auth;
        private readonly IMapper _mapper;
        public CartService(ICartRepository repository, IUserContextService auth,IMapper mapper)
        {
            _repository = repository;
            _auth = auth;
            _mapper = mapper;
        }

        public async Task<Result<CartItemDTO>> addCartItem([FromForm] cartAddDTO DTO)
        {
            var user = _auth.convertToInt(_auth.getValueAuth());
            if (user == 0) return Result<CartItemDTO>.Failure(Utils.Enum.StatusCode.Unauthorized);

            var cartItem = await _repository.addCartItem(DTO, user);
            if (cartItem.statusCode != Utils.Enum.StatusCode.Success) return Result<CartItemDTO>.Failure(Utils.Enum.StatusCode.Isvalid);
            //mapper
            try
            {
                var data = _mapper.Map<CartItemDTO>(cartItem.data);
                return Result<CartItemDTO>.Success(data);
            }
            catch
            {
                return Result<CartItemDTO>.Failure(Utils.Enum.StatusCode.Isvalid);
            }
            
        }

        public async Task<Result<List<CartItemDTO>>> getCart()
        {
            var user = _auth.convertToInt(_auth.getValueAuth());
            if (user == 0) return Result<List<CartItemDTO>>.Failure(Utils.Enum.StatusCode.Unauthorized);
            var data = await _repository.getCart(user);
            if(data is null) return Result<List<CartItemDTO>>.Failure(Utils.Enum.StatusCode.NotFound);
            try
            {
                var cartItem = _mapper.Map<List<CartItemDTO>>(data.data);
                return Result<List<CartItemDTO>>.Success(cartItem);
            }
            catch
            {
               return  Result<List<CartItemDTO>>.Failure(Utils.Enum.StatusCode.NotFound);
            }
        }

        public async Task<Result<CartItems>> removeCartItem([FromForm] cartDeleteDTO DTO)
        {
            var user = _auth.convertToInt(_auth.getValueAuth());
            if (user == 0) return Result<CartItems>.Failure(Utils.Enum.StatusCode.Unauthorized);
            var data = await _repository.removeCartItem(DTO, user);
            if (data.statusCode != Utils.Enum.StatusCode.Success) return Result<CartItems>.Failure(Utils.Enum.StatusCode.Isvalid);
            return Result<CartItems>.Success();
        }

        public async Task<Result<CartItemDTO>> updateCartItem([FromForm] CartItemDTO DTO)
        {
            var user = _auth.convertToInt(_auth.getValueAuth());
            if (user == 0) return Result<CartItemDTO>.Failure(Utils.Enum.StatusCode.Unauthorized);
            var data = await _repository.updateCartItem(DTO,user);
            if(data.statusCode != Utils.Enum.StatusCode.Success) return Result<CartItemDTO>.Failure(Utils.Enum.StatusCode.Isvalid);
            return Result<CartItemDTO>.Success(); 
        }
    }
}
