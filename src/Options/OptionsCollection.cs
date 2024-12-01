using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Vertical.SpectreLogger.Options
{
    /// <summary>
    /// Manages options for renderer types.
    /// </summary>
    public class OptionsCollection
    {
        private readonly ConcurrentDictionary<Type, object> _options = new();

        internal OptionsCollection()
        {
        }

        /// <summary>
        /// Clears all options from the collection.
        /// </summary>
        public void Clear() => _options.Clear();
        
        /// <summary>
        /// Configures an options object for a renderer type.
        /// </summary>
        /// <param name="configure">Delegate that configures the provided object.</param>
        /// <typeparam name="TOptions">Options type.</typeparam>
        /// <exception cref="ArgumentNullException"><paramref name="configure"/> delegate is null</exception>
        public void Configure<TOptions>(Action<TOptions> configure) where TOptions : new()
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }
            
            configure(GetOptions<TOptions>());
        }

        /// <summary>
        /// Retrieves the value of an options object.
        /// </summary>
        /// <typeparam name="TOptions">Options type.</typeparam>
        /// <returns></returns>
        public TOptions GetOptions<TOptions>() where TOptions : new()
        {
            var type = typeof(TOptions);

            return (TOptions)_options.GetOrAdd(type, new TOptions());
        }

        /// <inheritdoc /> 
        public override string ToString() => $"[{string.Join(",", _options.Values.Select(v => v.GetType().Name))}]";
    }
}