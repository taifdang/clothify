using AutoMapper;
using clothes_backend.DTO.General;
using clothes_backend.DTO.ORDER;
using clothes_backend.Interfaces;
using clothes_backend.Interfaces.Repository;
using clothes_backend.Interfaces.Service;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderReposiotry _repository;
        private readonly IUserContextService _authService;
        private readonly IMapper _mapper;
        public OrderService(IOrderReposiotry repository,IUserContextService authService,IMapper mapper)
        {
            _repository = repository;
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<Result<Orders>> add([FromForm] orderItemDTO DTO)
        {
            var user_id = _authService.convertToInt(_authService.getValueAuth());
            if (user_id == 0) return Result<Orders>.Failure(Utils.Enum.StatusCode.Unauthorized);
            try
            {
                var data = await _repository.add(DTO,user_id);
                if (data.statusCode != Utils.Enum.StatusCode.Success) return Result<Orders>.Failure(Utils.Enum.StatusCode.NotFound);            
                return Result<Orders>.Success();
            }
            catch
            {
                return Result<Orders>.Failure(Utils.Enum.StatusCode.Isvalid);
            }

        }

        public async Task<Result<List<orderDTO>>> getAll()
        {
            var user_id = _authService.convertToInt(_authService.getValueAuth());
            if (user_id == 0) return Result<List<orderDTO>>.Failure(Utils.Enum.StatusCode.Unauthorized);
            try
            {
                var data = await _repository.getAll(user_id);
                if(data.statusCode != Utils.Enum.StatusCode.Success) return Result<List<orderDTO>>.Failure(Utils.Enum.StatusCode.NotFound);
                var order = _mapper.Map<List<orderDTO>>(data.data);
                return Result<List<orderDTO>>.Success(order);
            }
            catch
            {
                return Result<List<orderDTO>>.Failure(Utils.Enum.StatusCode.Isvalid);
            }
        }

        public async Task<Result<List<orderDetailDTO>>> getId(int id)
        {
            var user_id = _authService.convertToInt(_authService.getValueAuth());
            if (user_id == 0) return Result<List<orderDetailDTO>>.Failure(Utils.Enum.StatusCode.Unauthorized);
            try
            {
                var data = await _repository.getId(id,user_id);
                if (data.statusCode != Utils.Enum.StatusCode.Success) return Result<List<orderDetailDTO>>.Failure(Utils.Enum.StatusCode.NotFound);
                var order = _mapper.Map<List<orderDetailDTO>>(data.data);
                return Result<List<orderDetailDTO>>.Success(order);
            }
            catch
            {
                return Result<List<orderDetailDTO>>.Failure(Utils.Enum.StatusCode.Isvalid);
            }
        }
    }
}
