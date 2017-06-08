using System;
using System.Threading.Tasks;

namespace Tasks.Database
{
    public interface IUnitOfWork : IDisposable
    {
        System.Data.Entity.Database Database { get; }

        #region Repository Interfaces (add one per entity)
        IGenericRepository<Models.Job> JobRepository { get; }
        #endregion
        Task<int> CommitAsync();
    }
}
