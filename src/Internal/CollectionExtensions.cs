﻿using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vertical.SpectreLogger.Internal
{
    internal static class CollectionExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool TryGetValue(
            this IReadOnlyList<KeyValuePair<string, object>> formattedLogValues,
            string key,
            out object? value)
        {
            var count = formattedLogValues.Count;

            for (var c = 0; c < count; c++)
            {
                // ReSharper disable once UseDeconstruction
                var  entry = formattedLogValues[c];

                if (key != entry.Key) 
                    continue;
                
                value = entry.Value;
                return true;
            }

            value = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static TValue? GetValueOrDefault<TKey, TValue>(
            this Dictionary<TKey, TValue> dictionary,
            TKey key,
            TValue? defaultValue)
            where TKey : notnull
        {
            return dictionary.TryGetValue(key, out var value)
                ? value
                : defaultValue;
        }
    }
}