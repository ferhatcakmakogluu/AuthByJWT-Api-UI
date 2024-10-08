using AuthByJWT.Core.Repositories;
using AuthByJWT.Core.Services;
using AuthByJWT.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthByJWT.Service.Services
{
    public class Service<T> : IService<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<T> _genericRepository;

        public Service(IUnitOfWork unitOfWork, IGenericRepository<T> genericRepository)
        {
            _unitOfWork = unitOfWork;
            _genericRepository = genericRepository;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _genericRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task Delete(T entity)
        {
            _genericRepository.Delete(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _genericRepository.GetAllAsync();
        }

        public Task<T> GetByIdAsync(string id)
        {
            return _genericRepository.GetByIdAsync(id);
        }

        public async Task Update(T entity)
        {
            _genericRepository.Update(entity);
            await _unitOfWork.CommitAsync();
        }
    }
}
