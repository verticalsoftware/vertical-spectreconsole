using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Rendering;
using Vertical.SpectreLogger.Tests.Infrastructure;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Rendering
{
    public class ScopeValuesRendererTests
    {
        [Theory, MemberData(nameof(Theories))]
        public void RenderWritesExpectedOutput(string template,
            Action<ScopeValuesRenderer.Options> rendererConfig,
            Func<ILogger, IDisposable> scopeStarter,
            string message,
            string expected)
        {
            var output = RendererTestHarness.Capture(config => config.ConfigureProfile(LogLevel.Information,
                    profile =>
                    {
                        profile.OutputTemplate = template;
                        profile.ClearTypeFormatters();
                        profile.ClearTypeStyles();
                        profile.DefaultLogValueStyle = null;
                        profile.ConfigureOptions(rendererConfig);
                    }),
                logger =>
                {
                    using (scopeStarter(logger))
                    {
                        logger.LogInformation(message);
                    }
                });
            
            output.ShouldBe(expected);
        }

        [Fact]
        public void RenderUsesContentBetween()
        {
            var output = RendererTestHarness.Capture(config => config.ConfigureProfile(LogLevel.Information,
                    profile =>
                    {
                        profile.OutputTemplate = "{Scopes}{Message}";
                        profile.ConfigureOptions<ScopeValuesRenderer.Options>(options =>
                            options.ContentBetween = ">");
                    }),
                logger =>
                {
                    using var scope1 = logger.BeginScope("First");
                    using var scope2 = logger.BeginScope("Second");
                    logger.LogInformation("test");
                });
            
            output.ShouldBe("[gold3_1]First[/]>[gold3_1]Second[/] => test");
        }
        
        [Fact]
        public void RenderUsesContentAfter()
        {
            var output = RendererTestHarness.Capture(config => config.ConfigureProfile(LogLevel.Information,
                    profile =>
                    {
                        profile.OutputTemplate = "{Scopes}{Message}";
                        profile.ConfigureOptions<ScopeValuesRenderer.Options>(options =>
                            options.ContentAfter = ">");
                    }),
                logger =>
                {
                    using var scope1 = logger.BeginScope("First");
                    using var scope2 = logger.BeginScope("Second");
                    logger.LogInformation("test");
                });
            
            output.ShouldBe("[gold3_1]First[/] => [gold3_1]Second[/]>test");
        }
        
        public static IEnumerable<object[]> Theories => new[]
        {
            new object[]
            {
                "{Scopes}{Message}",
                new Action<ScopeValuesRenderer.Options>(_ => { }),
                new Func<ILogger, IDisposable>(_ => Substitute.For<IDisposable>()),
                "scope",
                "scope"
            },
            new object[]
            {
                "{Scopes}{Message}",
                new Action<ScopeValuesRenderer.Options>(_ => {}),
                new Func<ILogger, IDisposable>(logger => logger.BeginScope("{x},{y}", 10, 20)!),
                "scope",
                "10,20 => scope"
            },
            new object[]
            {
                "{Scopes}{Message}",
                new Action<ScopeValuesRenderer.Options>(_ => {}),
                new Func<ILogger, IDisposable>(logger => logger.BeginScope(new {x=10,y=20})!),
                "scope",
                "{x: 10, y: 20} => scope"
            },
            new object[]
            {
                "{Scopes}{Message}",
                new Action<ScopeValuesRenderer.Options>(_ => {}),
                new Func<ILogger, IDisposable>(logger =>
                {
                    logger.BeginScope(new {x = 10, y = 20});
                    return logger.BeginScope("z = {z}", 30)!;
                }),
                "scope",
                "{x: 10, y: 20} => z = 30 => scope"
            },
            new object[]
            {
                "{Scopes}{Message}",
                new Action<ScopeValuesRenderer.Options>(opt => opt.ContentBefore = "scopes= "),
                new Func<ILogger, IDisposable>(logger => logger.BeginScope(new {x=10,y=20})!),
                "scope",
                "scopes= {x: 10, y: 20} => scope"
            },
            new object[]
            {
                "{Scopes}{Message}",
                new Action<ScopeValuesRenderer.Options>(opt => opt.ContentBetween = ">>>"),
                new Func<ILogger, IDisposable>(logger =>
                {
                    logger.BeginScope(new {x = 10, y = 20});
                    return logger.BeginScope("z = {z}", 30)!;
                }),
                "scope",
                "{x: 10, y: 20}>>>z = 30 => scope"
            },
            new object[]
            {
                "{Scopes}{Message}",
                new Action<ScopeValuesRenderer.Options>(opt => opt.ContentAfter = ">>>"),
                new Func<ILogger, IDisposable>(logger => logger.BeginScope(new {x=10,y=20})!),
                "scope",
                "{x: 10, y: 20}>>>scope"
            }
        };
    }
}