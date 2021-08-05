using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Options;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Encapsulates a single log event.
    /// </summary>
    public readonly ref struct LogEventInfo
    {
        public LogEventInfo(
            string categoryName,
            LogLevel logLevel,
            EventId eventId,
            object? state,
            Exception? exception,
            IDictionary<string, object?> formattedLogValues,
            IEnumerable<object?> scopes,
            FormattingProfile formattingProfile)
        {
            CategoryName = categoryName;
            LogLevel = logLevel;
            EventId = eventId;
            State = state;
            Exception = exception;
            FormattedLogValues = formattedLogValues;
            FormattingProfile = formattingProfile;
            Scopes = scopes.ToArray();
        }

        /// <summary>
        /// Gets the logger name.
        /// </summary>
        public string CategoryName { get; }

        /// <summary>
        /// Gets the log level of the event.
        /// </summary>
        public LogLevel LogLevel { get; }
        
        /// <summary>
        /// Gets the event id.
        /// </summary>
        public EventId EventId { get; }
        
        /// <summary>
        /// Gets the state object.
        /// </summary>
        public object? State { get; }
        
        /// <summary>
        /// Gets the exception.
        /// </summary>
        public Exception? Exception { get; }

        /// <summary>
        /// Gets the formatted log values.
        /// </summary>
        public IDictionary<string, object?> FormattedLogValues { get; }

        /// <summary>
        /// Gets the formatting profile.
        /// </summary>
        public FormattingProfile FormattingProfile { get; }

        /// <summary>
        /// Gets the scopes.
        /// </summary>
        public object?[] Scopes { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            var message = FormattedLogValues.GetValueOrDefault("{OriginalFormat}")?.ToString()
                          ?? "(LogEventData)";
            return $"{LogLevel}: {message}";
        }
    }
}