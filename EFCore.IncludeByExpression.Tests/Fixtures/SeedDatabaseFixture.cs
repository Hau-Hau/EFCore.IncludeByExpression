using System.Linq;
using EFCore.IncludeByExpression.Tests.Data;
using EFCore.IncludeByExpression.Tests.Data.Entieties;

namespace EFCore.IncludeByExpression.Tests.Fixtures
{
    public class SeedDatabaseFixture
    {
        public TestAppDbContext GetNewContext() => new();

        private static AEntity CreateA()
        {
            var instance = new AEntity();
            instance.Childs = new BEntity[]
            {
                CreateB(instance),
                CreateB(instance),
                CreateB(instance)
            }.ToList();
            return instance;
        }

        private static BEntity CreateB(AEntity parent)
        {
            var instance = new BEntity(parent);
            instance.Childs = new CEntity[]
            {
                CreateC(instance, parent),
                CreateC(instance, parent),
                CreateC(instance, parent),
            }.ToList();
            return instance;
        }

        private static CEntity CreateC(BEntity parent, AEntity parentAncestor)
        {
            var instance = new CEntity(parent, parent.Parent);
            instance.Childs = new DEntity[]
            {
                new DEntity(instance, parent),
                new DEntity(instance, parent),
                new DEntity(instance, parent),
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
