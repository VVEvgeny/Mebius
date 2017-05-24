using System.Data.Entity;

namespace Tasks
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext() : base("DbConnection")
        { }
        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
        }
    }
}
