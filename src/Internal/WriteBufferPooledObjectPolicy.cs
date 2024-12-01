using System;
using Microsoft.Extensions.ObjectPool;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Internal
{
    internal class WriteBufferPooledObjectPolicy : PooledObjectPolicy<IWriteBuffer>
    {
        private readonly IConsoleWriter _consoleWriter;
        private readonly Func<IWriteBuffer> _writeBufferFactory;

        internal WriteBufferPooledObjectPolicy(IConsoleWriter consoleWriter, Func<IWriteBuffer> writeBufferFactory)
        {
            _consoleWriter = consoleWriter ?? throw new ArgumentNullException(nameof(consoleWriter));
            _writeBufferFactory = writeBufferFactory ?? throw new ArgumentNullException(nameof(writeBufferFactory));
        }

        /// <inheritdoc />
        public override IWriteBuffer Create() => _writeBufferFactory();

        /// <inheritdoc />
        public override bool Return(IWriteBuffer obj)
        {
            obj.Flush();
            obj.Margin = 0;

            return true;
        }
    }
}