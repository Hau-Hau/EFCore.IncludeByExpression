using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DotNext.Runtime;

namespace EFCore.IncludeByExpression.Abstractions
{
    internal class SoftConcurrentDictionary<TKey, TValue> : IDictionary<TKey, TValue>
        where TKey : notnull
        where TValue : class
    {
        private readonly ConcurrentDictionary<TKey, SoftReference<TValue>> dictionary = new();

        public TValue this[TKey key]
        {
            get
            {
                SoftReference<TValue> softValue = dictionary[key];
                if (!softValue.TryGetTarget(out var target))
                {
                    throw new Exception("Target already collected.");
                }

                return target;
            }
            set => dictionary[key] = new SoftReference<TValue>(value);
        }

        private void EvictCollectedReferences()
        {
            foreach (var key in dictionary.Keys)
            {
                if (dictionary[key].TryGetTarget(out var target))
                {
                    continue;
                }

                dictionary.Remove(key, out var _);
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                EvictCollectedReferences();
                return dictionary.Keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                EvictCollectedReferences();
                return dictionary
                    .Values.Select(x =>
                    {
                        if (x.TryGetTarget(out var target))
                        {
                            return target;
                        }

                        return null;
                    })
                    .Where(x => x != null)
                    .ToList()!;
            }
        }

        public int Count => dictionary.Count;

        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value)
        {
            EvictCollectedReferences();
            dictionary.TryAdd(key, new SoftReference<TValue>(value));
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            EvictCollectedReferences();
            dictionary.TryAdd(item.Key, new SoftReference<TValue>(item.Value));
        }

        public void Clear()
        {
            dictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            EvictCollectedReferences();
            return dictionary.ContainsKey(item.Key);
        }

        public bool ContainsKey(TKey key)
        {
            EvictCollectedReferences();
            return dictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            EvictCollectedReferences();
            (
                dictionary
                    .Where(static kvp => kvp.Value.TryGetTarget(out var _))
                    .Select(static kvp =>
                    {
                        kvp.Value.TryGetTarget(out var target);
                        return KeyValuePair.Create(kvp.Key, target);
                    })
                    .ToArray()
            ).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            EvictCollectedReferences();
            foreach (var kvp in dictionary)
            {
                if (kvp.Value.TryGetTarget(out var target))
                {
                    yield return new KeyValuePair<TKey, TValue>(kvp.Key, target);
                }
            }
        }

        public bool Remove(TKey key)
        {
            EvictCollectedReferences();
            return dictionary.Remove(key, out var _);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            EvictCollectedReferences();
            return dictionary.Remove(item.Key, out var _);
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            EvictCollectedReferences();
            if (
                !dictionary.ContainsKey(key)
                || !dictionary.TryGetValue(key, out var softValue)
                || !softValue.TryGetTarget(out var target)
            )
            {
                value = default;
                return false;
            }

            value = target;
            return true;
        }

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            EvictCollectedReferences();
            if (key is null)
            {
                throw new KeyNotFoundException();
            }

            if (valueFactory is null)
            {
                throw new ArgumentNullException(nameof(valueFactory));
            }

            if (dictionary.ContainsKey(key) && dictionary[key].TryGetTarget(out var target))
            {
                return target;
            }

            var value = valueFactory(key);
            this[key] = value;
            return value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
