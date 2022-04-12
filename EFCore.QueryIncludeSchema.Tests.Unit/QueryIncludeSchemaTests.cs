using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using EFCore.QueryIncludeSchema.Interfaces;
using EFCore.QueryIncludeSchema.Tests.Unit.Fixtures;
using System.Linq;
using Xunit;

namespace EFCore.QueryIncludeSchema.Tests.Unit
{
    public class QueryIncludeSchemaTests : IClassFixture<SeedDatabaseFixture>
    {
        private readonly SeedDatabaseFixture seedDatabaseFixture;

        public QueryIncludeSchemaTests(SeedDatabaseFixture seedDatabaseFixture)
        {
            this.seedDatabaseFixture = seedDatabaseFixture;
        }

        [Fact]
        public void NoRelatedDataIncluded_ReturnsOnlyBase()
        {
            using var contextReference = seedDatabaseFixture.GetNewContext();
            using var context = seedDatabaseFixture.GetNewContext();
            var reference = contextReference.Bs.First();
            var node = QueryIncludeSchema.For(context.Bs).Execute().First();
            node.Childs.Should().BeNull();
            node.Parent.Should().BeNull();
            node.Should().BeEquivalentTo(reference);
        }

        [Fact]
        public void ParentIncluded_ParentExists()
        {
            using var contextReference = seedDatabaseFixture.GetNewContext();
            using var context = seedDatabaseFixture.GetNewContext();
            var reference = contextReference.Bs
                .Include(y => y.Parent)
                .First();
            var node = QueryIncludeSchema
                .For(context.Bs)
                .Execute(x => x.Include(y => y.Parent))
                .First();
            node.Parent.Should().NotBeNull();
            node.Should().BeEquivalentTo(reference, x => x.IgnoringCyclicReferences());
        }

        [Fact]
        public void ChildsIncluded_ChildsArrayIsNotNullOrEmpty()
        {
            using var contextReference = seedDatabaseFixture.GetNewContext();
            using var context = seedDatabaseFixture.GetNewContext();
            var reference = contextReference.Bs
                .Include(y => y.Childs)
                .First();
            var node = QueryIncludeSchema
                .For(context.Bs)
                .Execute(x => x.Include(y => y.Childs))
                .First();
            node.Parent.Should().BeNull();
            node.Childs.Should().NotBeNullOrEmpty();
            node.Childs!.First().Should().BeEquivalentTo(reference!.Childs!.First(), x => x.IgnoringCyclicReferences());
            node.Should().BeEquivalentTo(reference, x => x.IgnoringCyclicReferences());
        }

        [Fact]
        public void ParentsChildsIncluded_ParentAndParentsChildsAreExists()
        {
            using var contextReference = seedDatabaseFixture.GetNewContext();
            using var context = seedDatabaseFixture.GetNewContext();
            var reference = contextReference.Bs
                .Include(y => y.Parent)
                .ThenInclude(y => y!.Childs)
                .First();
            var node = QueryIncludeSchema
                .For(context.Bs)
                .Execute(x => x
                    .Include(y => y.Parent)
                    .ThenInclude(y => y.Childs))
                .First();
            node.Parent.Should().NotBeNull();
            node.Parent!.Childs.Should().NotBeNullOrEmpty();
            node.Parent!.Childs!.First().Should().BeEquivalentTo(reference.Parent!.Childs!.First(), x => x.IgnoringCyclicReferences());
            node.Should().BeEquivalentTo(reference, x => x.IgnoringCyclicReferences());
        }

        [Fact]
        public void IncludeAfterThenInclude_ResultIsSameAsFromEF()
        {
            using var contextReference = seedDatabaseFixture.GetNewContext();
            using var context = seedDatabaseFixture.GetNewContext();
            var reference = contextReference.Bs
                .Include(y => y.Parent)
                .ThenInclude(y => y!.Childs)
                .Include(x => x.Childs)
                .First();
            var node = QueryIncludeSchema
                .For(context.Bs)
                .Execute(x => x
                    .Include(y => y.Parent)
                    .ThenInclude(y => y.Childs)
                    .Include(x => x.Childs))
                .First();
            node.Parent.Should().NotBeNull();
            node.Childs.Should().NotBeNullOrEmpty();
            node.Parent!.Childs.Should().NotBeNullOrEmpty();
            node.Parent!.Childs!.First().Should().BeEquivalentTo(reference.Parent!.Childs!.First(), x => x.IgnoringCyclicReferences());
            node.Should().BeEquivalentTo(reference, x => x.IgnoringCyclicReferences());
        }

        [Fact]
        public void IncludeRelatedDataOfEnumerable_RelatedOfEnumerableExists()
        {
            using var contextReference = seedDatabaseFixture.GetNewContext();
            using var context = seedDatabaseFixture.GetNewContext();
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            var reference = contextReference.As
                .Include(y => y.Childs)
                .ThenInclude(y => y.Childs)
                .ThenInclude(y => y.Childs)
                .First();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            var node = QueryIncludeSchema
                .For(context.As)
                .Execute(x => x
                    .Include(y => y.Childs)
                    .ThenInclude(y => y.Childs)
                    .ThenInclude(y => y.Childs))
                .First();
            node.Childs!.First()!.Parent!.Childs!.First().Childs!.First().Childs.Should().NotBeNullOrEmpty();
            node.Childs!.First().Should().BeEquivalentTo(reference.Childs!.First(), x => x.IgnoringCyclicReferences());
            node.Should().BeEquivalentTo(reference, x => x.IgnoringCyclicReferences());
        }
    }
}