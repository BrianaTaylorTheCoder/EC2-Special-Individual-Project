using AutoMapper;
using System.Linq.Expressions;
using EC2_2100212.Services.Interfaces;
using EC2_2100212.Models;
using EC2_2100212.ViewModels;
using EC2_2100212.Repositories.Interfaces;


namespace EC2_2100212.Services.Implementations
{
    public class CategoryService : IGenericService<CategoryViewModel>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<IEnumerable<CategoryViewModel>> GetAll(Expression<Func<CategoryViewModel, bool>>? expression = null,
            Func<IQueryable<CategoryViewModel>, IOrderedQueryable<CategoryViewModel>>? orderBy = null, List<string>? includes = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllAsync()
        {
            var modelList = await _unitOfWork.GenericRepository<Category>().GetAllAsync();
            return _mapper.Map<List<CategoryViewModel>>(modelList);
        }

        public async Task<CategoryViewModel> GetByIdAsync(int id)
        {
            var model = await _unitOfWork.GenericRepository<Category>().GetByIdAsync(id);
            return _mapper.Map<CategoryViewModel>(model);
        }

        public async Task<CategoryViewModel> InsertAsync(CategoryViewModel entity)
        {
            var model = _mapper.Map<Category>(entity);
            await _unitOfWork.GenericRepository<Category>().InsertAsync(model);
            await _unitOfWork.Save();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.GenericRepository<Category>().DeleteAsync(id);
            await _unitOfWork.Save();
        }
        public async Task UpdateAsync(CategoryViewModel entity)
        {
            var model = _mapper.Map<Category>(entity);
            await _unitOfWork.GenericRepository<Category>().UpdateAsync(model);
            await _unitOfWork.Save();
        }
    }
}
