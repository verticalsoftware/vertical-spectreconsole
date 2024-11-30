using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Scopes;

namespace Vertical.SpectreLogger
{
    /// <summary>
    /// Implementation of the logger.
    /// </summary>
    public class SpectreLogger : ILogger
    {
        private readonly ILogEventFilter? _logEventFilter;
        private readonly IRendererPipeline _rendererPipeline;
        private readonly ScopeManager _scopeManager;
        private readonly string _categoryName;
        private readonly SpectreLoggerOptions _options;
        private readonly LogLevel _minimumLevel;

        internal SpectreLogger(
            IRendererPipeline rendererPipeline,
            SpectreLoggerOptions options,
            ScopeManager scopeManager,
            string categoryName)
        {
            _rendererPipeline = rendererPipeline;
            _scopeManager = scopeManager;
            _categoryName = categoryName;
            _options = options;
            _logEventFilter = _options.LogEventFilter;
            _minimumLevel = ResolveMinimumLevel(categoryName);
        }

        /// <inheritdoc />
        public void Log<TState>(
            LogLevel logLevel, 
            EventId eventId, 
            TState state, 
            Exception? exception, 
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var profile = _options.LogLevelProfiles[logLevel];
            var scopeValues = _scopeManager.GetValues();
            
            var eventInfo = new LogEventContext(
                _categoryName,
                logLevel,
                eventId,
                state,
                exception,
                scopeValues,
                profile);

            if (!(_logEventFilter?.Filter(eventInfo)).GetValueOrDefault(true))
                return;
            
            _rendererPipeline.Render(eventInfo);
        }


        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None && logLevel >= _minimumLevel;
        }

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state) where TState : notnull
        {
            return _scopeManager.BeginScope(state);
        }

        private LogLevel ResolveMinimumLevel(string categoryName)
        {
            var categoryMatch = _options
                .MinimumLevelOverrides
                .Where(kv => categoryName.StartsWith(kv.Key, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(kv => kv.Key.Length)
                .FirstOrDefault();

            return !string.IsNullOrWhiteSpace(categoryMatch.Key)
                ? categoryMatch.Value
                : _options.MinimumLogLevel;
        }
    }
}