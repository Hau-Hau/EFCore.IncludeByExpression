using System;
using System.Collections.Concurrent;
using System.Reflection;
using DotNext.Runtime;
using EFCore.IncludeByExpression.Abstractions;
using FluentAssertions;
using Xunit;

namespace EFCore.IncludeByExpression.Tests
{
    public class SoftConcurrentDictionaryTests
    {
        [Fact]
        public void Add_Item_ShouldBeRetrievable()
        {
            var dict = new SoftConcurrentDictionary<string, object>();
            var value = new object();
            dict.Add("key1", value);
            dict.TryGetValue("key1", out var result).Should().BeTrue();
            result.Should().Be(value);
        }

        [Fact]
        public void Indexer_ShouldGetAndSetItem()
        {
            var dict = new SoftConcurrentDictionary<string, object>();
            var value = new object();
            dict["key1"] = value;
            dict["key1"].Should().Be(value);
        }

        [Fact]
        public void TryGetValue_ShouldReturnFalseIfKeyNotFound()
        {
            var dict = new SoftConcurrentDictionary<string, object>();
            var result = dict.TryGetValue("nonexistent", out var value);
            result.Should().BeFalse();
            value.Should().BeNull();
        }

        [Fact]
        public void GetOrAdd_ShouldReturnExistingValue()
        {
            var dict = new SoftConcurrentDictionary<string, object>();
            var value = new object();
            dict.Add("key1", value);
            var result = dict.GetOrAdd("key1", key => new object());
            result.Should().Be(value);
        }

        [Fact]
        public void GetOrAdd_ShouldAddAndReturnNewValue()
        {
            var dict = new SoftConcurrentDictionary<string, object>();
            var newValue = new object();
            var result = dict.GetOrAdd("key1", key => newValue);
            result.Should().Be(newValue);
            dict.TryGetValue("key1", out var retrievedValue).Should().BeTrue();
            retrievedValue.Should().Be(newValue);
        }

        [Fact]
        public void Remove_ShouldRemoveItem()
        {
            var dict = new SoftConcurrentDictionary<string, object> { { "key1", new object() } };
            dict.Remove("key1").Should().BeTrue();
            dict.ContainsKey("key1").Should().BeFalse();
        }

        [Fact]
        public void Clear_ShouldRemoveAllItems()
        {
            var dict = new SoftConcurrentDictionary<string, object>
            {
                { "key1", new object() },
                { "key2", new object() },
            };
            dict.Clear();
            dict.Should().BeEmpty();
        }

        [Fact]
        public void ContainsKey_ShouldReturnTrueForExistingKey()
        {
            var dict = new SoftConcurrentDictionary<string, object> { { "key1", new object() } };
            dict.ContainsKey("key1").Should().BeTrue();
        }

        [Fact]
        public void ContainsKey_ShouldReturnFalseForNonExistingKey()
        {
            var dict = new SoftConcurrentDictionary<string, object>();

            dict.ContainsKey("nonexistent").Should().BeFalse();
        }

        [Fact]
        public void Keys_ShouldReturnAllKeys()
        {
            var dict = new SoftConcurrentDictionary<string, object>
            {
                { "key1", new object() },
                { "key2", new object() },
            };
            var keys = dict.Keys;
            keys.Should().Contain(new[] { "key1", "key2" });
        }

        [Fact]
        public void Values_ShouldReturnAllValues()
        {
            var dict = new SoftConcurrentDictionary<string, object>();
            var value1 = new object();
            var value2 = new object();

            dict.Add("key1", value1);
            dict.Add("key2", value2);

            dict.Values.Should().Contain(new[] { value1, value2 });
        }

        [Fact]
        public void Count_ShouldReturnCorrectCount()
        {
            var dict = new SoftConcurrentDictionary<string, object>
            {
                { "key1", new object() },
                { "key2", new object() },
            };
            dict.Count.Should().Be(2);
        }

        [Fact]
        public void Add_SameKey_ShouldThrowException()
        {
            var dict = new SoftConcurrentDictionary<string, object> { { "key1", new object() } };
            Action act = () => dict.Add("key1", new object());
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void EvictCollectedReferences_ShouldRemoveCollectedItems()
        {
            var dict = new SoftConcurrentDictionary<string, object> { { "key1", new object() } };

            var dictionaryField = typeof(SoftConcurrentDictionary<string, object>).GetField(
                "dictionary",
                BindingFlags.NonPublic | BindingFlags.Instance
            );

            var innerDictionary =
                dictionaryField!.GetValue(dict) as ConcurrentDictionary<string, SoftReference<object>>;
            var softValue = innerDictionary!["key1"];
            softValue.Clear();

            dict.TryGetValue("key1", out var result);
            result.Should().BeNull();
            dict.ContainsKey("key1").Should().BeFalse();
        }

        [Fact]
        public void Enumerator_ShouldReturnAllItems()
        {
            var dict = new SoftConcurrentDictionary<string, object>
            {
                { "key1", new object() },
                { "key2", new object() },
            };
            var enumerator = dict.GetEnumerator();
            var count = 0;
            while (enumerator.MoveNext())
            {
                count++;
            }
            count.Should().Be(2);
        }
    }
}
