using AutoMapper;
using System.Linq.Expressions;
using EC2_2100212.Services.Interfaces;
using EC2_2100212.ViewModels;
using EC2_2100212.Models;
using EC2_2100212.Repositories.Interfaces;

namespace EC2_2100212.Services.Implementations
{
    public class OrderService : IGenericService<OrderViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<IEnumerable<OrderViewModel>> GetAll(Expression<Func<OrderViewModel, bool>>? expression = null,
            Func<IQueryable<OrderViewModel>, IOrderedQueryable<OrderViewModel>>? orderBy = null, List<string>? includes = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<OrderViewModel>> GetAllAsync()
        {
            var modelList = await _unitOfWork.GenericRepository<Order>().GetAllAsync();
            return _mapper.Map<List<OrderViewModel>>(modelList);
        }
        
        public async Task<OrderViewModel> GetByIdAsync(int id)
        {
            var model = await _unitOfWork.GenericRepository<Order>().GetByIdAsync(id);
            return _mapper.Map<OrderViewModel>(model);
        }

        public async Task<OrderViewModel> InsertAsync(OrderViewModel entity)
        {
            var model = _mapper.Map<Order>(entity);
            await _unitOfWork.GenericRepository<Order>().InsertAsync(model);
            await _unitOfWork.Save();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.GenericRepository<Order>().DeleteAsync(id);
            await _unitOfWork.Save();
        }
        public async Task UpdateAsync(OrderViewModel entity)
        {
            var model = _mapper.Map<Order>(entity);
            await _unitOfWork.GenericRepository<Order>().UpdateAsync(model);
            await _unitOfWork.Save();
        }
    }
}
