using Vertical.SpectreLogger.Core;

namespace Vertical.SpectreLogger.Scopes
{
    internal static class ScopeValues
    {
        internal static IScopeValues Create(LoggerScope? scope)
        {
            return scope switch
            {
                {PreviousScope: { }} => new MultiScopeValues(scope),
                { } => new SingleScopeValue(scope),
                _ => EmptyScopeValues.Default
            };
        }
    }
}