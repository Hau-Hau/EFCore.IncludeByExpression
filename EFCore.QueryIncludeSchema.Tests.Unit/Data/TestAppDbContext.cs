using Microsoft.EntityFrameworkCore;
using EFCore.QueryIncludeSchema.Tests.Unit.Data.Entieties;

namespace EFCore.QueryIncludeSchema.Tests.Unit.Data
{
    public class TestAppDbContext : DbContext
    {
        public TestAppDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase("TestAppDbContext");
        }

        public DbSet<AEntity> As { get; set; } = null!;
        public DbSet<BEntity> Bs { get; set; } = null!;
        public DbSet<CEntity> Cs { get; set; } = null!;
        public DbSet<CEntity> Ds { get; set; } = null!;
    }
}
