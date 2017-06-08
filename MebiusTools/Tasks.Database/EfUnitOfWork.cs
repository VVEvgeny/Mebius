using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Threading.Tasks;

namespace Tasks.Database
{
    public class EfUnitOfWork : DbContext, IUnitOfWork
    {
        public new System.Data.Entity.Database Database { get; private set; }

        #region Private Repos (add one per entity)

        private EfGenericRepository<Models.Job> _jobRepo;

        #endregion

        #region Public DbSets (add one per entity)

        public DbSet<Models.Job> Jobs { get; set; }

        #endregion

        #region Constructor

        public EfUnitOfWork() : base("DBConnection")
        {
            Database = base.Database;
        }

        #endregion

        #region IUnitOfWork Implementation (add one per entity)

        public IGenericRepository<Models.Job> JobRepository => _jobRepo ?? (_jobRepo = new EfGenericRepository<Models.Job>(Jobs));

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
            /*
            System.Data.Entity.Database.SetInitializer<EfUnitOfWork>(null);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Job>();
            */
        }

        #endregion

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
