using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger
{
    public class SpectreLogger : ILogger
    {
        private readonly ILogEventFilter? _logEventFilter;
        private readonly IRendererPipeline _rendererPipeline;
        private readonly ObjectPool<IWriteBuffer> _writeBufferPool;
        private readonly ScopeManager _scopeManager;
        private readonly SpectreLoggerOptions _options;

        internal SpectreLogger(
            IRendererPipeline rendererPipeline,
            SpectreLoggerOptions options,
            ObjectPool<IWriteBuffer> writeBufferPool,
            ScopeManager scopeManager)
        {
            _rendererPipeline = rendererPipeline;
            _writeBufferPool = writeBufferPool;
            _scopeManager = scopeManager;
            _options = options;
            _logEventFilter = _options.LogEventFilter;
        }
        
        /// <inheritdoc />
        public void Log<TState>(
            LogLevel logLevel, 
            EventId eventId, 
            TState state, 
            Exception exception, 
            Func<TState, Exception, string> formatter)
        {
            if (ReferenceEquals(null, state))
            {
                // Nothing to render?
                return;
            }
            
            var eventInfo = new LogEventInfo(
                logLevel,
                eventId,
                state,
                exception,
                _options.LogLevelProfiles[logLevel]);

            if (_logEventFilter?.Filter(eventInfo) == true)
                return;

            var writeBuffer = _writeBufferPool.Get();

            try
            {
                _rendererPipeline.Render(writeBuffer, eventInfo);
            }
            finally
            {
                _writeBufferPool.Return(writeBuffer);
            }
        }

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel > LogLevel.None && logLevel >= _options.MinimumLogLevel;
        }

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state)
        {
            return _scopeManager.BeginScope(state);
        }
    }
}