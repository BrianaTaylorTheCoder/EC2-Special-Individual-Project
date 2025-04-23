using AutoMapper;
using System.Linq.Expressions;
using EC2_2100212.Services.Interfaces;
using EC2_2100212.ViewModels;
using EC2_2100212.Models;
using EC2_2100212.Repositories.Interfaces;

namespace EC2_2100212.Services.Implementations
{
    public class BagCategoryService : IGenericService<BagCategoryViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public BagCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<IEnumerable<BagCategoryViewModel>> GetAll(Expression<Func<BagCategoryViewModel, bool>>? expression = null,
            Func<IQueryable<BagCategoryViewModel>, IOrderedQueryable<BagCategoryViewModel>>? orderBy = null, List<string>? includes = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BagCategoryViewModel>> GetAllAsync()
        {
            var modelList = await _unitOfWork.GenericRepository<BagCategory>().GetAllAsync();
            return _mapper.Map<List<BagCategoryViewModel>>(modelList);
        }

        public async Task<BagCategoryViewModel> GetByIdAsync(int id)
        {
            var model = await _unitOfWork.GenericRepository<BagCategory>().GetByIdAsync(id);
            return _mapper.Map<BagCategoryViewModel>(model);
        }

        public async Task<BagCategoryViewModel> InsertAsync(BagCategoryViewModel entity)
        {
            var model = _mapper.Map<BagCategory>(entity);
            await _unitOfWork.GenericRepository<BagCategory>().InsertAsync(model);
            await _unitOfWork.Save();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.GenericRepository<BagCategory>().DeleteAsync(id);
            await _unitOfWork.Save();
        }
        public async Task UpdateAsync(BagCategoryViewModel entity)
        {
            var model = _mapper.Map<BagCategory>(entity);
            await _unitOfWork.GenericRepository<BagCategory>().UpdateAsync(model);
            await _unitOfWork.Save();
        }
    }
}
