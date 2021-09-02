using System;
using System.Text.RegularExpressions;

namespace Vertical.SpectreLogger.Templates
{
    public static class TemplateString
    {
        /// <summary>
        /// Splits a string into template segments.
        /// </summary>
        /// <param name="str">String to split.</param>
        /// <param name="callback">A callback that receives each <see cref="TemplateSegment"/></param>
        /// <exception cref="ArgumentNullException"><paramref name="str"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="callback"/> is null.</exception>
        public static void Split(string str, TemplateCallback callback)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            const string pattern =
                @"(?<!{){(?<"
                + TemplateSegment.InnerTemplateGroup
                + @">(?<" 
                + TemplateSegment.KeyGroup
                + @">@?[^,:{}]+)(?<"
                + TemplateSegment.CompositeFormatSpanGroup 
                + @">(?<" 
                + TemplateSegment.WidthSpanGroup 
                + @">,(?<" 
                + TemplateSegment.WidthValueGroup 
                + @">-?\d+))?(?<" 
                + TemplateSegment.FormatSpanGroup 
                + ">:(?<" 
                + TemplateSegment.FormatValueGroup 
                + ">[^}]+))?))}";
            
            var match = Regex.Match(str, pattern);
            var position = 0;

            for (; match.Success; match = match.NextMatch())
            {
                if (match.Index > position)
                {
                    // Report non-match segment
                    callback(new TemplateSegment(null, str, position, match.Index - position));
                }

                callback(new TemplateSegment(match, str, match.Index, match.Length));
                position = match.Index + match.Length;
            }

            if (position < str.Length)
            {
                callback(new TemplateSegment(null, str, position, str.Length - position));
            }
        }
    }
}