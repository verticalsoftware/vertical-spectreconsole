﻿using System;
using System.Collections.Generic;
using System.Globalization;

namespace Vertical.SpectreLogger.Formatting
{
    /// <summary>
    /// Represents a <see cref="ICustomFormatter"/> that uses the profile type
    /// formatters.
    /// </summary>
    internal class TypeFormatter : ICustomFormatter
    {
        private readonly Dictionary<Type, ICustomFormatter> _typeFormatters;

        internal TypeFormatter(Dictionary<Type, ICustomFormatter> typeFormatters)
        {
            _typeFormatters = typeFormatters;
        }

        /// <inheritdoc />
        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            if (arg == null)
            {
                return string.Empty;
            }

            if (_typeFormatters.TryGetValue(arg.GetType(), out var typeFormatter))
            {
                return typeFormatter.Format(format, arg, formatProvider);
            }

            if (arg is IFormattable formattableValue)
            {
                return formattableValue.ToString(format, CultureInfo.CurrentCulture);
            }

            return arg.ToString() ?? string.Empty;
        }
    }
}