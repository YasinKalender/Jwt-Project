using JwtTokenProject.Common.Responses;
using JwtTokenProject.DAL.Repositories;
using JwtTokenProject.DAL.Uow;
using JwtTokenProject.Service.Interfaces;
using JwtTokenProject.Service.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JwtTokenProject.Service.Services
{
    public class Service<T, TDto> : IService<T, TDto> where T : class where TDto : class,new()
    {
        private readonly IUnitOfWork _unitOfWork;
        public IRepository<T> _repository;

        public Service(IUnitOfWork unitOfWork, IRepository<T> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<Response<TDto>> AddAsync(TDto entity)
        {
            var newEntity = ObjectMapper.Mapper.Map<T>(entity);
            await _repository.AddAsync(newEntity);
            await _unitOfWork.SaveChangesAsync();
            return Response<TDto>.Success(entity, StatusCodes.Status201Created);
        }

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
            var entity = await _repository.GetAllAsync();
            var entityMap = ObjectMapper.Mapper.Map<IEnumerable<TDto>>(entity);

            return Response<IEnumerable<TDto>>.Success(entityMap, StatusCodes.Status200OK);

        }

        public async Task<Response<TDto>> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            var entityMap = ObjectMapper.Mapper.Map<TDto>(entity);

            return Response<TDto>.Success(entityMap, StatusCodes.Status200OK);
        }

        public async Task<Response<TDto>> RemoveAsync(TDto entity)
        {
            var entityMap = ObjectMapper.Mapper.Map<T>(entity);
            _repository.Remove(entityMap);

            await _unitOfWork.SaveChangesAsync();

            return Response<TDto>.Success(entity, StatusCodes.Status200OK);
        }

        public async Task<Response<TDto>> UpdateAsync(TDto entity)
        {
            var entityMap = ObjectMapper.Mapper.Map<T>(entity);
            _repository.Update(entityMap);

            await _unitOfWork.SaveChangesAsync();

            return Response<TDto>.Success(entity, StatusCodes.Status200OK);
        }

        public async Task<Response<IQueryable<TDto>>> WhereAsync(Expression<Func<T, bool>> predicate)
        {
            var query =await _repository.Where(predicate).ToListAsync();
            var entityMap = ObjectMapper.Mapper.Map<IQueryable<TDto>>(query);

            return Response<IQueryable<TDto>>.Success(entityMap, StatusCodes.Status200OK);
        }
    }
}
