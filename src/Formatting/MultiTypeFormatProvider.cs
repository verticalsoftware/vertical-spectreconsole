using System;

namespace Vertical.SpectreLogger.Formatting
{
    /// <summary>
    /// Represents a provider that returns a <see cref="MultiTypeFormatter"/>
    /// </summary>
    internal class MultiTypeFormatProvider : IFormatProvider
    {
        private readonly ICustomFormatter _typeFormatter;

        internal MultiTypeFormatProvider(ICustomFormatter typeFormatter)
        {
            _typeFormatter = typeFormatter ?? throw new ArgumentNullException(nameof(typeFormatter));
        }
        
        /// <inheritdoc />
        public object? GetFormat(Type? formatType)
        {
            return typeof(ICustomFormatter) == formatType
                ? _typeFormatter
                : null;
        }
    }
}