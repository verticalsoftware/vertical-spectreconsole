using System;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Options;

namespace Vertical.SpectreLogger.Core
{
    /// <summary>
    /// Defines the properties of a log event.
    /// </summary>
    public readonly struct LogEventContext
    {
        internal LogEventContext(
            string categoryName,
            LogLevel logLevel,
            EventId eventId,
            object? state,
            Exception? exception,
            IScopeValues scopeValues,
            LogLevelProfile profile)
        {
            CategoryName = categoryName;
            LogLevel = logLevel;
            EventId = eventId;
            State = state;
            Exception = exception;
            ScopeValues = scopeValues;
            Profile = profile;
        }

        /// <summary>
        /// Gets the category name of the logger that received the event.
        /// </summary>
        public string CategoryName { get; }

        /// <summary>
        /// Gets the log level.
        /// </summary>
        public LogLevel LogLevel { get; }

        /// <summary>
        /// Gets the event id.
        /// </summary>
        public EventId EventId { get; }
        
        /// <summary>
        /// Gets the event state data.
        /// </summary>
        public object? State { get; }
        
        /// <summary>
        /// Gets the exception.
        /// </summary>
        public Exception? Exception { get; }

        /// <summary>
        /// Gets the log event scope values.
        /// </summary>
        public IScopeValues ScopeValues { get; }

        /// <summary>
        /// Gets the log level profile.
        /// </summary>
        public LogLevelProfile Profile { get; }
    }
}