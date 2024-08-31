using System.Linq;
using EFCore.IncludeByExpression.Abstractions;
using EFCore.IncludeByExpression.Tests.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EFCore.IncludeByExpression.Tests
{
    public class IncludeByExpressionTests : IClassFixture<SeedDatabaseFixture>
    {
        private readonly SeedDatabaseFixture seedDatabaseFixture;

        public IncludeByExpressionTests(SeedDatabaseFixture seedDatabaseFixture)
        {
            this.seedDatabaseFixture = seedDatabaseFixture;
        }

        [Fact]
        public void WhenNoRelatedDataIncluded_ThenReturnsOnlyBase()
        {
            using var contextReference = seedDatabaseFixture.GetNewContext();
            using var context = seedDatabaseFixture.GetNewContext();
            var reference = contextReference.Bs.First();
            var node = context.Bs.IncludeByExpression().First();
            node.Childs.Should().BeNull();
            node.Parent.Should().BeNull();
            node.Should().BeEquivalentTo(reference);
        }

        [Fact]
        public void WhenParentIncluded_ThenParentExists()
        {
            using var contextReference = seedDatabaseFixture.GetNewContext();
            using var context = seedDatabaseFixture.GetNewContext();
            var reference = contextReference.Bs.Include(y => y.Parent).First();
            var node = context.Bs.IncludeByExpression(x => x.Include(y => y.Parent)).First();
            node.Parent.Should().NotBeNull();
            node.Should().BeEquivalentTo(reference, x => x.IgnoringCyclicReferences());
        }

        [Fact]
        public void WhenMultipleParentsIncluded_ThenMultipleParentsExists()
        {
            using var contextReference = seedDatabaseFixture.GetNewContext();
            using var context = seedDatabaseFixture.GetNewContext();
            var reference = contextReference
                .Ds.Include(x => x.Parent)
                .Include(x => x.ParentAncestor)
                .First();
            var node = context
                .Ds.IncludeByExpression(x => x.Include(y => y.Parent))
                .IncludeByExpression(x => x.Include(y => y.ParentAncestor))
                .First();
            node.Parent.Should().NotBeNull();
            node.ParentAncestor.Should().NotBeNull();
            node.Should().BeEquivalentTo(reference, x => x.IgnoringCyclicReferences());
        }

        [Fact]
        public void WhenMultipleParentsWithChildsIncluded_ThenMultipleParentsWithChildsExists()
        {
            using var contextReference = seedDatabaseFixture.GetNewContext();
            using var context = seedDatabaseFixture.GetNewContext();
            var reference = contextReference
                .Ds.Include(x => x.Childs)
                .Include(x => x.Parent)
                .ThenInclude(x => x.Childs)
                .Include(x => x.ParentAncestor)
                .ThenInclude(x => x.Childs)
                .First();
            var node = context
                .Ds.IncludeByExpression(x => x.Include(y => y.Childs))
                .IncludeByExpression(x => x.Include(y => y.Parent).ThenInclude(x => x.Childs))
                .IncludeByExpression(x =>
                    x.Include(y => y.ParentAncestor).ThenInclude(x => x.Childs)
                )
                .First();
            node.Childs.Should().NotBeNullOrEmpty();
            node.Childs!.First()
                .Should()
                .BeEquivalentTo(reference!.Childs!.First(), x => x.IgnoringCyclicReferences());
            node.Parent.Should().NotBeNull();
            node.Parent.Childs.Should().NotBeNullOrEmpty();
            node.Parent.Childs!.First()
                .Should()
                .BeEquivalentTo(
                    reference!.Parent.Childs!.First(),
                    x => x.IgnoringCyclicReferences()
                );
            node.ParentAncestor.Should().NotBeNull();
            node.ParentAncestor.Childs.Should().NotBeNullOrEmpty();
            node.ParentAncestor.Childs!.First()
                .Should()
                .BeEquivalentTo(
                    reference!.ParentAncestor.Childs!.First(),
                    x => x.IgnoringCyclicReferences()
                );
            node.Should().BeEquivalentTo(reference, x => x.IgnoringCyclicReferences());
        }

        [Fact]
        public void WhenChildsIncluded_ThenChildsArrayIsNotNullOrEmpty()
        {
            using var contextReference = seedDatabaseFixture.GetNewContext();
            using var context = seedDatabaseFixture.GetNewContext();
            var reference = contextReference.Bs.Include(y => y.Childs).First();
            var node = context.Bs.IncludeByExpression(x => x.Include(y => y.Childs)).First();
            node.Parent.Should().BeNull();
            node.Childs.Should().NotBeNullOrEmpty();
            node.Childs!.First()
                .Should()
                .BeEquivalentTo(reference!.Childs!.First(), x => x.IgnoringCyclicReferences());
            node.Should().BeEquivalentTo(reference, x => x.IgnoringCyclicReferences());
        }

        [Fact]
        public void WhenParentsChildsIncluded_ThenParentAndParentsChildsAreExists()
        {
            using var contextReference = seedDatabaseFixture.GetNewContext();
            using var context = seedDatabaseFixture.GetNewContext();
            var reference = contextReference
                .Bs.Include(y => y.Parent)
                .ThenInclude(y => y!.Childs)
                .First();
            var node = context
                .Bs.IncludeByExpression(x => x.Include(y => y.Parent).ThenInclude(y => y.Childs))
                .First();
            node.Parent.Should().NotBeNull();
            node.Parent!.Childs.Should().NotBeNullOrEmpty();
            node.Parent!.Childs!.First()
                .Should()
                .BeEquivalentTo(
                    reference.Parent!.Childs!.First(),
                    x => x.IgnoringCyclicReferences()
                );
            node.Should().BeEquivalentTo(reference, x => x.IgnoringCyclicReferences());
        }

        [Fact]
        public void WhenIncludeAfterThenInclude_ThenResultIsSameAsFromEF()
        {
            using var contextReference = seedDatabaseFixture.GetNewContext();
            using var context = seedDatabaseFixture.GetNewContext();
            var reference = contextReference
                .Bs.Include(y => y.Parent)
                .ThenInclude(y => y!.Childs)
                .Include(x => x.Childs)
                .First();
            var node = context
                .Bs.IncludeByExpression(x =>
                    x.Include(y => y.Parent).ThenInclude(y => y.Childs).Include(x => x.Childs)
                )
                .First();
            node.Parent.Should().NotBeNull();
            node.Childs.Should().NotBeNullOrEmpty();
            node.Parent!.Childs.Should().NotBeNullOrEmpty();
            node.Parent!.Childs!.First()
                .Should()
                .BeEquivalentTo(
                    reference.Parent!.Childs!.First(),
                    x => x.IgnoringCyclicReferences()
                );
            node.Should().BeEquivalentTo(reference, x => x.IgnoringCyclicReferences());
        }

        [Fact]
        public void WhenIncludeRelatedDataOfEnumerable_ThenRelatedOfEnumerableExists()
        {
            using var contextReference = seedDatabaseFixture.GetNewContext();
            using var context = seedDatabaseFixture.GetNewContext();
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            var reference = contextReference
                .As.Include(y => y.Childs)
                .ThenInclude(y => y.Childs)
                .ThenInclude(y => y.Childs)
                .First();
            var node = context
                .As.IncludeByExpression(x =>
                    x.Include(y => y.Childs).ThenInclude(y => y.Childs).ThenInclude(y => y.Childs)
                )
                .First();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            node.Childs!.First()!
                .Parent!.Childs!.First()
                .Childs!.First()
                .Childs.Should()
                .NotBeNullOrEmpty();
            node.Childs!.First()
                .Should()
                .BeEquivalentTo(reference.Childs!.First(), x => x.IgnoringCyclicReferences());
            node.Should().BeEquivalentTo(reference, x => x.IgnoringCyclicReferences());
        }
    }
}
