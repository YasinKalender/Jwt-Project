using JwtTokenProject.Common.Responses;
using System.Linq.Expressions;

namespace JwtTokenProject.Service.Interfaces
{
    public interface IService<T, TDto> where T : class where TDto : class
    {
        Task<Response<TDto>> GetByIdAsync(int id);
        Task<Response<IEnumerable<TDto>>> GetAllAsync();
        Task<Response<IQueryable<TDto>>> WhereAsync(Expression<Func<T, bool>> predicate);
        Task<Response<TDto>> AddAsync(TDto entity);
        Task<Response<TDto>> RemoveAsync(TDto entity);
        Task<Response<TDto>> UpdateAsync(TDto entity);
    }
}
