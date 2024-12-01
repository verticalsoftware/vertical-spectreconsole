using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Shouldly;
using Vertical.SpectreLogger.Rendering;
using Vertical.SpectreLogger.Tests.Infrastructure;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Rendering
{
    public class CategoryNameRendererTests
    {
        private readonly ICustomFormatter _testInstance = new CategoryNameRenderer.DefaultFormatter();

        [Fact]
        public void RenderWritesExpectedValue()
        {
            RendererTestHarness.RunScenario(
                config => config.ConfigureProfiles(p => p.OutputTemplate = "{CategoryName}"),
                logger => logger.LogInformation(""),
                $@"^\[\w+\]{nameof(CategoryNameRendererTests)}\[/\]$",
                nameof(CategoryNameRendererTests));
        }
        
        [Theory, MemberData(nameof(Theories))]
        public void FormatReturnsExpected(string? format, string arg, string expected)
        {
            _testInstance.Format(format, new CategoryNameRenderer.Value(arg), null).ShouldBe(expected);
        }

        public static IEnumerable<object?[]> Theories => new[]
        {
            new object?[]{ null, "Logger", "Logger" },
            new object?[]{ null, "", "" },
            new object?[]{ "C", "Logger", "Logger" },
            new object?[]{ "C", "System.Logger", "Logger" },
            new object?[]{ "S1", "System.Logger", "Logger" },
            new object?[]{ "S2", "System.Logger", "System.Logger" },
            new object?[]{ "S2", "System.Logging.Logger", "Logging.Logger" }
        };
    }
}