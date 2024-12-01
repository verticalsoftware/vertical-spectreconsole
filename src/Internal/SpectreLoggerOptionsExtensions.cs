using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Options;

namespace Vertical.SpectreLogger.Internal
{
    internal static class SpectreLoggerOptionsExtensions
    {
        internal static LogLevel GetLogLevelFilter(this SpectreLoggerOptions options,
            string loggerCategory)
        {
            var bestMatch = options
                .MinimumLevelOverrides
                .Where(kv => loggerCategory.StartsWith(kv.Key, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(kv => kv.Key.Length)
                .FirstOrDefault();

            return bestMatch is { Key: not null } && bestMatch.Value > options.MinimumLogLevel
                ? bestMatch.Value
                : options.MinimumLogLevel;
        }
    }
}