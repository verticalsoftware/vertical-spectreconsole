using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Shouldly;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Tests.Infrastructure;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Rendering
{
    public class ScopeValueRendererTests
    {
        [Theory, MemberData(nameof(Theories))]
        public void RenderWritesExpectedOutput(string template, 
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
        
        public static IEnumerable<object[]> Theories => new[]
        {
            new object[]
            {
                "{Scope=scope}{Message}",
                new Func<ILogger, IDisposable>(log => log.BeginScope("test")!),
                "no scope",
                "no scope"
            },
            new object[]
            {
                "({Scope=x},{Scope=y}) {Message}",
                new Func<ILogger, IDisposable>(log => log.BeginScope(new Dictionary<string, object>
                {
                    ["x"]=10,["y"]=20
                })!),
                "mouse clicked",
                "(10,20) mouse clicked"
            },
            new object[]
            {
                "({Scope=x},{Scope=y}) {Message}",
                new Func<ILogger, IDisposable>(log => log.BeginScope(new[]
                {
                    new KeyValuePair<string,object>("x", 10),
                    new KeyValuePair<string,object>("y", 20)
                })!),
                "mouse clicked",
                "(10,20) mouse clicked"
            },
            new object[]
            {
                "({Scope=x},{Scope=y}) {Message}",
                new Func<ILogger, IDisposable>(log =>
                {
                    // Test IEnumerable
                    var queue = new Queue<KeyValuePair<string, object>>();
                    queue.Enqueue(new KeyValuePair<string, object>("x",10));
                    queue.Enqueue(new KeyValuePair<string, object>("y",20));
                    return log.BeginScope(queue)!;
                }),
                "mouse clicked",
                "(10,20) mouse clicked"
            },
            new object[]
            {
                "({Scope=x},{Scope=y}) {Message}",
                new Func<ILogger, IDisposable>(log => log.BeginScope("Mouse is at {x},{y}", 10, 20)!),
                "mouse clicked",
                "(10,20) mouse clicked"
            }
        };
    }
}