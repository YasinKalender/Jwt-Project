using JwtTokenProject.DAL.Context;

namespace JwtTokenProject.DAL.Uow
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProjectContext _projectContext;

        public UnitOfWork(ProjectContext projectContext)
        {
            _projectContext = projectContext;
        }

        public void SaveChanges()
        {
            _projectContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _projectContext.SaveChangesAsync();
        }
    }
}
