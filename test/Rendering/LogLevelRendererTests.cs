using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Shouldly;
using Vertical.SpectreLogger.Tests.Infrastructure;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Rendering
{
    public class LogLevelRendererTests
    {
        [Theory, MemberData(nameof(Theories))]
        public void RenderWritesExpectedValue(LogLevel logLevel, string expected)
        {
            var output = RendererTestHarness.Capture(
                config =>
                {
                    config.ConfigureProfiles(profile =>
                    {
                        profile.DefaultLogValueStyle = null;
                        profile.OutputTemplate = "{LogLevel}";
                    });
                    config.SetMinimumLevel(LogLevel.Trace);
                },
                logger => logger.Log(logLevel, ""));
            
            output.ShouldBe(expected);
        }
        
        public static IEnumerable<object[]> Theories = new[]
        {
            new object[] {LogLevel.Trace, "Trce"},
            new object[] {LogLevel.Debug, "Dbug"},
            new object[] {LogLevel.Information, "Info"},
            new object[] {LogLevel.Warning, "Warn"},
            new object[] {LogLevel.Error, "Fail"},
            new object[] {LogLevel.Critical, "Crit"},
        };
    }
}