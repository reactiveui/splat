// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>Shared dictionary helpers for the callback registries maintained by the service-location resolvers.</summary>
internal static class ResolverDictionaryHelpers
{
    /// <summary>Returns the value stored under <paramref name="key"/>, creating and inserting a fresh instance when absent.</summary>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type; a reference type with a parameterless constructor.</typeparam>
    /// <param name="dictionary">The dictionary to read from or insert into.</param>
    /// <param name="key">The key whose value should be retrieved or created.</param>
    /// <returns>The existing value for <paramref name="key"/>, or the newly created value that was just inserted.</returns>
    /// <remarks>
    /// On <c>net6.0</c> and later a single hash lookup is performed via
    /// <c>CollectionsMarshal.GetValueRefOrAddDefault</c>; on earlier targets it falls back to a get-then-add pair.
    /// Because the returned reference is the same instance stored in the dictionary, callers mutate it in place.
    /// Callers are expected to hold whatever gate protects <paramref name="dictionary"/>.
    /// </remarks>
    internal static TValue GetOrAddValue<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key)
        where TKey : notnull
        where TValue : class, new()
    {
#if NET6_0_OR_GREATER
        ref var slot = ref System.Runtime.InteropServices.CollectionsMarshal.GetValueRefOrAddDefault(dictionary, key, out _);
        return slot ??= new();
#else
        if (!dictionary.TryGetValue(key, out var value))
        {
            value = new();
            dictionary[key] = value;
        }

        return value;
#endif
    }
}
