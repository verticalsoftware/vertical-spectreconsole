using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Spectre.Console;
using Spectre.Console.Rendering;
using Vertical.SpectreLogger.Output;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Output
{
    public class BackgroundConsoleWriterTests
    {
        [Fact]
        public async Task WriteCapturesContent()
        {
            var console = Substitute.For<IAnsiConsole>();
            var services = new ServiceCollection()
                .AddLogging(builder => builder.AddSpectreConsole(
                    config =>
                    {
                        config.WriteInBackground();
                        config.ConfigureProfile(LogLevel.Information, profile =>
                        {
                            profile.DefaultLogValueStyle = null;
                        });
                    }))
                .AddSingleton(console)
                .BuildServiceProvider();

            var logger = services.GetRequiredService<ILogger<BackgroundConsoleWriterTests>>();
            
            logger.LogInformation("Test event successful");

            await services.DisposeAsync();
            
            console.Received().Write(Arg.Any<IRenderable>());
        }
        
        [Fact]
        public void WritePushedContentAfterDispose()
        {
            var console = Substitute.For<IAnsiConsole>();
            var writer = new BackgroundConsoleWriter(console);
            
            writer.Dispose();
            writer.Write("test");
            
            console.Received().Write(Arg.Any<IRenderable>());
        }
    }
}