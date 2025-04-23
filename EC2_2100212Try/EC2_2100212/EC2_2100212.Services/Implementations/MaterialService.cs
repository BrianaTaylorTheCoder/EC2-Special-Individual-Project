using AutoMapper;
using System.Linq.Expressions;
using EC2_2100212.Services.Interfaces;
using EC2_2100212.Models;
using EC2_2100212.ViewModels;
using EC2_2100212.Repositories.Interfaces;


namespace EC2_2100212.Services.Implementations
{
    public class MaterialService : IGenericService<MaterialViewModel>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public MaterialService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<IEnumerable<MaterialViewModel>> GetAll(Expression<Func<MaterialViewModel, bool>>? expression = null,
            Func<IQueryable<MaterialViewModel>, IOrderedQueryable<MaterialViewModel>>? orderBy = null, List<string>? includes = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MaterialViewModel>> GetAllAsync()
        {
            var modelList = await _unitOfWork.GenericRepository<Material>().GetAllAsync();
            return _mapper.Map<List<MaterialViewModel>>(modelList);
        }

        public async Task<MaterialViewModel> GetByIdAsync(int id)
        {
            var model = await _unitOfWork.GenericRepository<Material>().GetByIdAsync(id);
            return _mapper.Map<MaterialViewModel>(model);
        }

        public async Task<MaterialViewModel> InsertAsync(MaterialViewModel entity)
        {
            var model = _mapper.Map<Material>(entity);
            await _unitOfWork.GenericRepository<Material>().InsertAsync(model);
            await _unitOfWork.Save();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.GenericRepository<Material>().DeleteAsync(id);
            await _unitOfWork.Save();
        }
        public async Task UpdateAsync(MaterialViewModel entity)
        {
            var model = _mapper.Map<Material>(entity);
            await _unitOfWork.GenericRepository<Material>().UpdateAsync(model);
            await _unitOfWork.Save();
        }
    }
}
