using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Renders the logger category.
    /// </summary>
    public partial class CategoryNameRenderer : ITemplateRenderer
    {
        private readonly TemplateSegment _template;

        /// <summary>
        /// Defines the template for this renderer.
        /// </summary>
        [Template]
        public static readonly string Template = TemplatePatternBuilder
            .ForKey("[Cc]ategory[Nn]ame")
            .AddAlignment()
            .AddFormatting()
            .Build();

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="template">Matching segment from the output template.</param>
        public CategoryNameRenderer(TemplateSegment template) => _template = template;
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventContext context)
        {
            buffer.WriteLogValue(
                context.Profile,
                _template,
                new Value(context.CategoryName));
        }
    }
}