using AutoMapper;
using System.Linq.Expressions;
using EC2_2100212.Services.Interfaces;
using EC2_2100212.ViewModels;
using EC2_2100212.Models;
using EC2_2100212.Repositories.Interfaces;

namespace EC2_2100212.Services.Implementations
{
    public class BagService : IGenericService<BagViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public BagService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<IEnumerable<BagViewModel>> GetAll(Expression<Func<BagViewModel, bool>>? expression = null,
            Func<IQueryable<BagViewModel>, IOrderedQueryable<BagViewModel>>? orderBy = null, List<string>? includes = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BagViewModel>> GetAllAsync()
        {
            var modelList = await _unitOfWork.GenericRepository<Bag>().GetAllAsync();
            return _mapper.Map<List<BagViewModel>>(modelList);
        }

        public async Task<BagViewModel> GetByIdAsync(int id)
        {
            var model = await _unitOfWork.GenericRepository<Bag>().GetByIdAsync(id);
            return _mapper.Map<BagViewModel>(model);
        }

        public async Task<BagViewModel> InsertAsync(BagViewModel entity)
        {
            var model = _mapper.Map<Bag>(entity);
            await _unitOfWork.GenericRepository<Bag>().InsertAsync(model);
            await _unitOfWork.Save();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.GenericRepository<Bag>().DeleteAsync(id);
            await _unitOfWork.Save();
        }
        public async Task UpdateAsync(BagViewModel entity)
        {
            var model = _mapper.Map<Bag>(entity);
            await _unitOfWork.GenericRepository<Bag>().UpdateAsync(model);
            await _unitOfWork.Save();
        }
    }
}
