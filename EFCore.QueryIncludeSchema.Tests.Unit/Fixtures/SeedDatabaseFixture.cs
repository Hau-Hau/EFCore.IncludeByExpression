using EFCore.QueryIncludeSchema.Tests.Unit.Data;
using EFCore.QueryIncludeSchema.Tests.Unit.Data.Entieties;
using System.Linq;

namespace EFCore.QueryIncludeSchema.Tests.Unit.Fixtures
{

    public class SeedDatabaseFixture
    {
        public TestAppDbContext GetNewContext() => new();

        private static AEntity CreateA()
        {
            var instance = new AEntity();
            instance.Childs = new BEntity[] {
                CreateB(instance),
                CreateB(instance),
                CreateB(instance)
            }.ToList();
            return instance;
        }

        private static BEntity CreateB(AEntity parent)
        {
            var instance = new BEntity(parent);
            instance.Childs = new CEntity[] {
                CreateC(instance),
                CreateC(instance),
                CreateC(instance),
            }.ToList();
            return instance;
        }

        private static CEntity CreateC(BEntity parent)
        {
            var instance = new CEntity(parent);
            instance.Childs = new DEntity[] {
                new DEntity(instance),
                new DEntity(instance),
                new DEntity(instance),
            }.ToList();
            return instance;
        }

        public SeedDatabaseFixture()
        {
            using var context = new TestAppDbContext();
            for (var i = 0; i < 99; i++)
            {
                context.As.Add(CreateA());
            }
            context.SaveChanges();
        }
    }
}