namespace Vertical.SpectreLogger.Core
{
    /// <summary>
    /// Defines a renderer pipeline.
    /// </summary>
    public interface IRendererPipeline
    {
        /// <summary>
        /// Renders the log event.
        /// </summary>
        /// <param name="logEventContext">Log event info</param>
        void Render(in LogEventContext logEventContext);
    }
}