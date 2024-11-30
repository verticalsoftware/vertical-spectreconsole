using Microsoft.Extensions.Logging;
using Shouldly;
using Vertical.SpectreLogger.Tests.Infrastructure;
using Xunit;

namespace Vertical.SpectreLogger.Tests
{
    public class SpectreLoggerTests
    {
        [Fact]
        public void LoggerDoesNotOutputWhenLevelNotEnabled()
        {
            RendererTestHarness.Capture(
                    cfg => cfg.SetMinimumLevel(LogLevel.Information),
                    logger => logger.LogDebug("test"))
                .ShouldBeEmpty();
        }

        [Fact]
        public void LoggerOutputsWhenLevelIsEnabled()
        {
            RendererTestHarness.Capture(
                    cfg => cfg
                        .SetMinimumLevel(LogLevel.Debug)
                        .ConfigureProfiles(pro =>
                        {
                            pro.DefaultLogValueStyle = null;
                            pro.OutputTemplate = "{Message}";
                        }),
                    logger => logger.LogDebug("test message"))
                .ShouldBe("test message");
        }

        [Fact]
        public void LoggerEmitsWhenEventIsAboveMinimumOverride()
        {
            RendererTestHarness.Capture(
                    cfg =>
                    {
                        cfg.SetMinimumLevel(LogLevel.Information);
                        cfg.SetMinimumLevel("Minimum", LogLevel.Warning);
                        cfg.ConfigureProfiles(profile => profile.OutputTemplate = "{Message}");
                    },
                    log => log.LogWarning("warning"),
                    "Minimum")
                .ShouldBe("warning");
        }

        [Fact]
        public void LoggerDoesNotEmitWhenEventIsBelowMinimumOverride()
        {
            RendererTestHarness.Capture(
                    cfg =>
                    {
                        cfg.SetMinimumLevel(LogLevel.Information);
                        cfg.SetMinimumLevel("Minimum", LogLevel.Warning);
                        cfg.ConfigureProfiles(profile => profile.OutputTemplate = "{Message}");
                    },
                    log => log.LogInformation("information"),
                    "Minimum")
                .ShouldBeEmpty();
        }

        private const bool Filtered = true;
        private const bool Emitted = false;

        [Theory]
        [InlineData("Microsoft.Hosting", LogLevel.Information, "Microsoft.Hosting", LogLevel.Information, Emitted)]
        [InlineData("Microsoft.Hosting", LogLevel.Information, "Microsoft.Hosting", LogLevel.Warning, Emitted)]
        [InlineData("Microsoft.Hosting", LogLevel.Warning, "Microsoft.Hosting", LogLevel.Information, Filtered)]
        [InlineData("Microsoft.Hosting", LogLevel.Warning, "Microsoft.Hosting", LogLevel.Warning, Emitted)]
        [InlineData("Microsoft.Hosting", LogLevel.Error, "Microsoft.Hosting", LogLevel.Debug, Filtered)]
        [InlineData("Microsoft.Hosting", LogLevel.Error, "Microsoft.Hosting", LogLevel.Information, Filtered)]
        [InlineData("Microsoft.Hosting", LogLevel.Error, "Microsoft.Hosting", LogLevel.Warning, Filtered)]
        [InlineData("Microsoft.Hosting", LogLevel.Error, "Microsoft.Hosting", LogLevel.Error, Emitted)]
        [InlineData("Microsoft.Hosting", LogLevel.Information, "Microsoft.Hosting.LifeTime", LogLevel.Information, Emitted)]
        [InlineData("Microsoft.Hosting", LogLevel.Warning, "Microsoft.Hosting.LifeTime", LogLevel.Information, Filtered)]
        [InlineData("Microsoft.Hosting", LogLevel.Warning, "Microsoft.Hosting.Internal", LogLevel.Information, Filtered)]
        [InlineData("Microsoft.Hosting", LogLevel.Error, "Microsoft.Hosting.Internal", LogLevel.Information, Filtered)]
        [InlineData("Microsoft.Data", LogLevel.Error, "Microsoft.Hosting.Internal", LogLevel.Information, Emitted)]
        public void LoggerEmitsExpectedForPatternMatchedMinimumOverride(
            string overrideCategory,
            LogLevel overrideLevel,
            string eventCategory,
            LogLevel eventLevel,
            bool expected)
        {
             var captured = RendererTestHarness.Capture(
                cfg =>
                {
                    cfg.SetMinimumLevel(LogLevel.Information);
                    cfg.SetMinimumLevel(overrideCategory, overrideLevel);
                    cfg.ConfigureProfiles(profile => profile.OutputTemplate = "{Message}");
                },
                log => log.Log(eventLevel, "event"),
                loggerName: eventCategory);
             
             string.IsNullOrEmpty(captured).ShouldBe(expected);
        }

        [Fact]
        public void LoggerPreservesFormatStringWhenConfigured()
        {
            RendererTestHarness.Capture(
                cfg => cfg.ConfigureProfiles(profiles =>
                {
                    profiles.PreserveMarkupInFormatStrings = true;
                    profiles.OutputTemplate = "{Message}";
                }),
                log => log.LogInformation("Here is [yellow]yellow[/], and here is [green1]green[/]."),
                "Program")
                .ShouldBe("Here is [yellow]yellow[/], and here is [green1]green[/].");            
        }
    }
}