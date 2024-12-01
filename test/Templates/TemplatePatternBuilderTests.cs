using System.Collections.Generic;
using System.Text.RegularExpressions;
using Shouldly;
using Vertical.SpectreLogger.Templates;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Templates
{
    public class TemplatePatternBuilderTests
    {
        [Fact]
        public void SplitPatternFieldReturnsExpectedPattern()
        {
            var pattern = TemplatePatternBuilder.SplitPattern;

            Should.NotThrow(() => new Regex(pattern));
            
            pattern.ShouldBe(@"(?<!{){(?<_tmpl>(?<_key>(?<_ds>@)?[^,:{}>]+)(?:(?<_ctl>[^,:{}]+))?(?<_cfmt>(?<_wdspan>,(?<_wd>-?\d+))?(?<_fmspan>:(?<_fm>[^}]+))?))}");
        }

        [Theory, MemberData(nameof(Theories))]
        public void ValueReturnsExpectedPattern(TemplatePatternBuilder patternBuilder, string expected)
        {
            var pattern = patternBuilder.Build();

            Should.NotThrow(() => new Regex(pattern));

            pattern.ShouldBe(expected);
        }

        public static IEnumerable<object[]> Theories = new[]
        {
            new object[]
            {
                TemplatePatternBuilder.ForKey("Template"),
                @"(?<!{){(?<_tmpl>(?<_key>Template))}"
            },
            new object[]
            {
                TemplatePatternBuilder.ForKey("Template")
                    .AddControlPattern(@"\d+"),
                @"(?<!{){(?<_tmpl>(?<_key>Template)(?:(?<_ctl>\d+))?)}"
            },
            new object[]
            {
                TemplatePatternBuilder.ForKey("Template")
                    .AddControlPattern(@"\d+")
                    .AddAlignment(),
                @"(?<!{){(?<_tmpl>(?<_key>Template)(?:(?<_ctl>\d+))?(?<_cfmt>(?<_wdspan>,(?<_wd>-?\d+))?))}"
            },
            new object[]
            {
                TemplatePatternBuilder.ForKey("Template")
                    .AddControlPattern(@"\d+")
                    .AddAlignment()
                    .AddFormatting(),
                @"(?<!{){(?<_tmpl>(?<_key>Template)(?:(?<_ctl>\d+))?(?<_cfmt>(?<_wdspan>,(?<_wd>-?\d+))?(?<_fmspan>:(?<_fm>[^}]+))?))}"
            }
        };
    }
}