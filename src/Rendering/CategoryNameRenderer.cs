﻿using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Formatting;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    public partial class CategoryNameRenderer : ITemplateRenderer
    {
        private readonly TemplateSegment _template;

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