namespace Vertical.SpectreLogger.Rendering
{
    public partial class ExceptionRenderer
    {
        /// <summary>
        /// Options for <see cref="ExceptionRenderer"/>
        /// </summary>
        public class Options
        {
            /// <summary>
            /// Gets or sets whether to unwind inner exceptions.
            /// </summary>
            public bool UnwindInnerExceptions { get; set; } = true;

            /// <summary>
            /// Gets or sets the maximum number of stack frames to show per exception.
            /// </summary>
            public int MaxStackFrames { get; set; } = 5;

            /// <summary>
            /// Gets or sets the number of characters to indent on each stack frame output.
            /// </summary>
            public int StackFrameIndent { get; set; } = 3;
            
            /// <summary>
            /// Gets or sets whether to display the parameter type in stack frame methods.
            /// </summary>
            public bool ShowParameterTypes { get; set; } = true;

            /// <summary>
            /// Gets or sets whether to display the parameter name in stack frame methods.
            /// </summary>
            public bool ShowParameterNames { get; set; } = true;

            /// <summary>
            /// Gets or sets whether to display the file name of a stack frame method.
            /// </summary>
            public bool ShowSourcePaths { get; set; } = true;

            /// <summary>
            /// Gets or sets whether to display the line number in files names of a stack frame
            /// method.
            /// </summary>
            public bool ShowSourceLocations { get; set; } = true;
        }
    }
}