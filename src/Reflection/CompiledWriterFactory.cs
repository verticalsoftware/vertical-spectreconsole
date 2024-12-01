using System;
using System.Reflection;
using Vertical.SpectreLogger.Destructuring;

namespace Vertical.SpectreLogger.Reflection
{
    internal abstract class CompiledWriterFactory
    {
        internal static readonly MethodInfo WriteStartObjectMethod = typeof(IDestructuringWriter)
            .GetMethod(nameof(DestructuringWriter.WriteStartObject))!;
        
        internal static readonly MethodInfo WriteEndObjectMethod = typeof(IDestructuringWriter)
            .GetMethod(nameof(DestructuringWriter.WriteEndObject))!;
        
        internal static readonly MethodInfo WritePropertyMethod = typeof(IDestructuringWriter)
            .GetMethod(nameof(DestructuringWriter.WriteProperty))!;
        
        internal static readonly MethodInfo WriteStartArrayMethod = typeof(IDestructuringWriter)
            .GetMethod(nameof(DestructuringWriter.WriteStartArray))!;
        
        internal static readonly MethodInfo WriteEndArrayMethod = typeof(IDestructuringWriter)
            .GetMethod(nameof(DestructuringWriter.WriteEndArray))!;
        
        internal static readonly MethodInfo WriteElementMethod = typeof(IDestructuringWriter)
            .GetMethod(nameof(DestructuringWriter.WriteElement))!;
        
        internal static readonly Type[] EmptyTypes = Array.Empty<Type>();
        
        internal static CompiledWriter? CreateWriter(Type type)
        {
            if (DictionaryWriterFactory.TryCreate(type, out var implementation))
                return implementation!;

            if (EnumerableWriterFactory.TryCreate(type, out implementation))
                return implementation!;

            if (ObjectWriterFactory.TryCreate(type, out implementation))
                return implementation!;

            return null;
        }
    }
}