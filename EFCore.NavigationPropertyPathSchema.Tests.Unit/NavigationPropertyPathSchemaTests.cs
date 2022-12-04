using EFCore.NavigationPropertyPathSchema.Abstractions;
using EFCore.NavigationPropertyPathSchema.Tests.Unit.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace EFCore.NavigationPropertyPathSchema.Tests.Unit
{
    public class NavigationPropertyPathSchemaTests : IClassFixture<SeedDatabaseFixture>
    {
        private readonly SeedDatabaseFixture seedDatabaseFixture;

        public NavigationPropertyPathSchemaTests(SeedDatabaseFixture seedDatabaseFixture)
        {
            this.seedDatabaseFixture = seedDatabaseFixture;
        }

        [Fact]
        public void WhenNoRelatedDataIncluded_ThenReturnsOnlyBase()
        {
            using var contextReference = seedDatabaseFixture.GetNewContext();
            using var context = seedDatabaseFixture.GetNewContext();
            var reference = contextReference.Bs.First();
            var node = NavigationPropertyPathSchema.For(context.Bs).Execute().First();
            node.Childs.Should().BeNull();
            node.Parent.Should().BeNull();
            node.Should().BeEquivalentTo(reference);
        }

        [Fact]
        public void WhenParentIncluded_ThenParentExists()
        {
            using var contextReference = seedDatabaseFixture.GetNewContext();
            using var context = seedDatabaseFixture.GetNewContext();
            var reference = contextReference.Bs
                .Include(y => y.Parent)
                .First();
            var node = NavigationPropertyPathSchema
                .For(context.Bs)
                .Execute(x => x.Include(y => y.Parent))
                .First();
            node.Parent.Should().NotBeNull();
            node.Should().BeEquivalentTo(reference, x => x.IgnoringCyclicReferences());
        }

        [Fact]
        public void WhenChildsIncluded_ThenChildsArrayIsNotNullOrEmpty()
        {
            using var contextReference = seedDatabaseFixture.GetNewContext();
            using var context = seedDatabaseFixture.GetNewContext();
            var reference = contextReference.Bs
                .Include(y => y.Childs)
                .First();
            var node = NavigationPropertyPathSchema
                .For(context.Bs)
                .Execute(x => x.Include(y => y.Childs))
                .First();
            node.Parent.Should().BeNull();
            node.Childs.Should().NotBeNullOrEmpty();
            node.Childs!.First().Should().BeEquivalentTo(reference!.Childs!.First(), x => x.IgnoringCyclicReferences());
            node.Should().BeEquivalentTo(reference, x => x.IgnoringCyclicReferences());
        }

        [Fact]
        public void WhenParentsChildsIncluded_ThenParentAndParentsChildsAreExists()
        {
            using var contextReference = seedDatabaseFixture.GetNewContext();
            using var context = seedDatabaseFixture.GetNewContext();
            var reference = contextReference.Bs
                .Include(y => y.Parent)
                .ThenInclude(y => y!.Childs)
                .First();
            var node = NavigationPropertyPathSchema
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
        public void WhenIncludeAfterThenInclude_ThenResultIsSameAsFromEF()
        {
            using var contextReference = seedDatabaseFixture.GetNewContext();
            using var context = seedDatabaseFixture.GetNewContext();
            var reference = contextReference.Bs
                .Include(y => y.Parent)
                .ThenInclude(y => y!.Childs)
                .Include(x => x.Childs)
                .First();
            var node = NavigationPropertyPathSchema
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
        public void WhenIncludeRelatedDataOfEnumerable_ThenRelatedOfEnumerableExists()
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
            var node = NavigationPropertyPathSchema
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