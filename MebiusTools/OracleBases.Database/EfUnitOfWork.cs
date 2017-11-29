using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using OracleBases.Database.Migrations;
using OracleBases.Database.Models;

namespace OracleBases.Database
{
    public class EfUnitOfWork : DbContext, IUnitOfWork
    {
        public new System.Data.Entity.Database Database { get; private set; }

        #region Private Repos (add one per entity)

        private EfGenericRepository<Connect> _connectRepo;

        #endregion

        #region Public DbSets (add one per entity)

        public DbSet<Connect> Connects { get; set; }

        #endregion

        #region Constructor

        public EfUnitOfWork() : base("DBConnection")
        {
            Database = base.Database;
            System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<EfUnitOfWork, Configuration>("DBConnection"));
            //Database.Log = s => Debug.InfoAsync(s); //System.Diagnostics.Debug.WriteLine(s);
        }
        public EfUnitOfWork(DbConnection connection) : base(connection, true)
        {
            Database = base.Database;
            System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<EfUnitOfWork, Configuration>(connection.Database));
            //Database.Log = s => Debug.InfoAsync(s); //System.Diagnostics.Debug.WriteLine(s);
        }

        #endregion

        #region IUnitOfWork Implementation (add one per entity)

        public IGenericRepository<Connect> ConnectRepository => _connectRepo ?? (_connectRepo = new EfGenericRepository<Connect>(Connects));

        #endregion

        public Task<int> CommitAsync()
        {
            try
            {
                return SaveChangesAsync();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);

                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        #region Code First Overrides

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //System.Data.Entity.Database.SetInitializer<EfUnitOfWork>(null);
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Entity<Job>();
        }

        #endregion

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
