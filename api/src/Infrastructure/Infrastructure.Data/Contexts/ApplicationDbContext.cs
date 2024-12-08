using Homemap.Domain.Core;
using Homemap.Domain.Devices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Homemap.Infrastructure.Data.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public required DbSet<Project> Projects { get; set; }

        public required DbSet<Receiver> Receivers { get; set; }

        public required DbSet<Device> Devices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // https://learn.microsoft.com/en-us/answers/questions/2101757/applying-migration-fails-with-error
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.NonTransactionalMigrationOperationWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            RegisterEntityDerivedTypes<Device, ACDevice>(modelBuilder);
        }

        // got from one of my previous projects
        //  https://github.com/e3stpavel/health-care-contacts/blob/main/src/Infrastructure/Infrastructure.Data/Contexts/ApplicationDbContext.cs
        private static void RegisterEntityDerivedTypes<TBase, TDerived>(ModelBuilder modelBuilder)
        {
            Type someDerivedType = typeof(TDerived);

            foreach (Type type in someDerivedType.Assembly.GetTypes()
                .Where(t => t.Namespace == someDerivedType.Namespace && t.IsClass && t.IsSubclassOf(typeof(TBase))))
            {
                modelBuilder.Entity(type);
            }
        }
    }
}
