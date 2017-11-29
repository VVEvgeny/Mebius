using System;
using System.Threading.Tasks;
using OracleBases.Database.Models;

namespace OracleBases.Database
{
    public interface IUnitOfWork : IDisposable
    {
        System.Data.Entity.Database Database { get; }

        #region Repository Interfaces (add one per entity)
        IGenericRepository<Connect> ConnectRepository { get; }

        #endregion
        Task<int> CommitAsync();
    }
}
