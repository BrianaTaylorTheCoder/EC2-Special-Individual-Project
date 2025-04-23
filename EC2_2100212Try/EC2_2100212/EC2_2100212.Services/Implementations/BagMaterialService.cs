using AutoMapper;
using System.Linq.Expressions;
using EC2_2100212.Services.Interfaces;
using EC2_2100212.ViewModels;
using EC2_2100212.Models;
using EC2_2100212.Repositories.Interfaces;

namespace EC2_2100212.Services.Implementations
{
    public class BagMaterialService : IGenericService<BagMaterialViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public BagMaterialService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<IEnumerable<BagMaterialViewModel>> GetAll(Expression<Func<BagMaterialViewModel, bool>>? expression = null,
            Func<IQueryable<BagMaterialViewModel>, IOrderedQueryable<BagMaterialViewModel>>? orderBy = null, List<string>? includes = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BagMaterialViewModel>> GetAllAsync()
        {
            var modelList = await _unitOfWork.GenericRepository<BagMaterial>().GetAllAsync();
            return _mapper.Map<List<BagMaterialViewModel>>(modelList);
        }

        public async Task<BagMaterialViewModel> GetByIdAsync(int id)
        {
            var model = await _unitOfWork.GenericRepository<BagMaterial>().GetByIdAsync(id);
            return _mapper.Map<BagMaterialViewModel>(model);
        }

        public async Task<BagMaterialViewModel> InsertAsync(BagMaterialViewModel entity)
        {
            var model = _mapper.Map<BagMaterial>(entity);
            await _unitOfWork.GenericRepository<BagMaterial>().InsertAsync(model);
            await _unitOfWork.Save();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.GenericRepository<BagMaterial>().DeleteAsync(id);
            await _unitOfWork.Save();
        }
        public async Task UpdateAsync(BagMaterialViewModel entity)
        {
            var model = _mapper.Map<BagMaterial>(entity);
            await _unitOfWork.GenericRepository<BagMaterial>().UpdateAsync(model);
            await _unitOfWork.Save();
        }
    }
}
