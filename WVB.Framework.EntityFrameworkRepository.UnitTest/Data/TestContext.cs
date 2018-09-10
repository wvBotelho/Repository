using Microsoft.EntityFrameworkCore;
using WVB.Framework.EntityFrameworkRepository.Configuration;
using WVB.Framework.EntityFrameworkRepository.Enum;
using WVB.Framework.EntityFrameworkRepository.UnitTest.Mapping;
using WVB.Framework.EntityFrameworkRepository.UnitTest.Models;

namespace WVB.Framework.EntityFrameworkRepository.UnitTest.Data
{
    public class TestContext : ProjectContext
    {
        public DbSet<Project> Projects { get; private set; }

        public DbSet<Customer> Customers { get; private set; }

        public DbSet<Resource> Resources { get; private set; }

        public DbSet<Technology> Technologies { get; private set; }

        public DbSet<ProjectDetail> ProjectDetails { get; private set; }

        public DbSet<ProjectResource> ProjectResources { get; private set; }

        public DbSet<ContactInformation> ContactInformation { get; private set; }

        public TestContext(Database database, string connectionString) : base(database, connectionString)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProjectMap());
            modelBuilder.ApplyConfiguration(new CustomerMap());
            modelBuilder.ApplyConfiguration(new ResourceMap());
            modelBuilder.ApplyConfiguration(new TechnologyMap());
            modelBuilder.ApplyConfiguration(new ProjectDetailMap());
            modelBuilder.ApplyConfiguration(new ProjectResourceMap());

            //global query filter
            modelBuilder.Entity<Project>().HasQueryFilter(p => EF.Property<bool>(p, "deletado") == false);
            modelBuilder.Entity<Customer>().HasQueryFilter(c => EF.Property<bool>(c, "deletado") == false);
            modelBuilder.Entity<Resource>().HasQueryFilter(r => EF.Property<bool>(r, "deletado") == false);
            modelBuilder.Entity<Technology>().HasQueryFilter(t => EF.Property<bool>(t, "deletado") == false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
