using EFCore.IncludeByExpression.Tests.Data.Entieties;
using Microsoft.EntityFrameworkCore;

namespace EFCore.IncludeByExpression.Tests.Data
{
    public class TestAppDbContext : DbContext
    {
        public TestAppDbContext() { }

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
