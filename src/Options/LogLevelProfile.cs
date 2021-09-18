using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Formatting;
using Vertical.SpectreLogger.Internal;

namespace Vertical.SpectreLogger.Options
{
    /// <summary>
    /// Defines options to be applied to a specific log level.
    /// </summary>
    public class LogLevelProfile
    {
        private readonly string _internalId = Guid.NewGuid().ToString("N");
        private ICustomFormatter? _formatter;
        private IFormatProvider? _formatProvider;

        internal LogLevelProfile(LogLevel logLevel)
        {
            LogLevel = logLevel;
        }

        /// <summary>
        /// Gets the log level.
        /// </summary>
        public LogLevel LogLevel { get; }

        /// <summary>
        /// Gets or sets the output template.
        /// </summary>
        public string? OutputTemplate { get; set; } = default!;

        /// <summary>
        /// Gets a dictionary of <see cref="ICustomFormatter"/> for value types.
        /// </summary>
        public Dictionary<Type, ICustomFormatter> TypeFormatters { get; } = new();

        /// <summary>
        /// Gets a dictionary of markup to apply before a specific value is rendered.
        /// </summary>
        public Dictionary<object, string> ValueStyles { get; } = new();

        /// <summary>
        /// Gets a dictionary of markup to apply before a value of a specific type is rendered.
        /// </summary>
        public Dictionary<Type, string> TypeStyles { get; } = new();

        /// <summary>
        /// Gets the style to apply before rendering a log value when no value or type
        /// style is matched.
        /// </summary>
        public string? DefaultLogValueStyle { get; set; }

        /// <summary>
        /// Gets a dictionary of option objects for renderers.
        /// </summary>
        internal OptionsCollection ConfiguredOptions { get; } = new();

        /// <summary>
        /// Gets the custom formatter.
        /// </summary>
        internal ICustomFormatter Formatter => _formatter ??= new MultiTypeFormatter(TypeFormatters);

        /// <summary>
        /// Gets the format provider.
        /// </summary>
        internal IFormatProvider FormatProvider => _formatProvider ??= new MultiTypeFormatProvider(Formatter);

        /// <summary>
        /// Gets a cache of objects available internally.
        /// </summary>
        internal RuntimeCacheCollection RuntimeCache { get; } = new();

        /// <inheritdoc />
        public override string ToString() => LogLevel.ToString();
    }
}