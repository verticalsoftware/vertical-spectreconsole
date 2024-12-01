using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using NSubstitute;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Rendering;
using Vertical.SpectreLogger.Templates;
using Vertical.SpectreLogger.Tests.Infrastructure;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Rendering
{
    public class RendererPipelineTests
    {
        private readonly SpectreLoggerOptions _options = new SpectreLoggerOptions();
        private readonly IWriteBuffer _writeBuffer = Substitute.For<IWriteBuffer>();
        private readonly ObjectPool<IWriteBuffer> _pool = Substitute.For<ObjectPool<IWriteBuffer>>();
        private readonly ITemplateRendererBuilder _rendererBuilder = Substitute.For<ITemplateRendererBuilder>();
        private readonly ITemplateRenderer[] _renderers = Factory.New<ITemplateRenderer>(
            () => Substitute.For<ITemplateRenderer>(), 5).ToArray();

        public RendererPipelineTests()
        {
            _pool.Get().Returns(_writeBuffer);
            _rendererBuilder.GetRenderers(Arg.Any<string>()).Returns(_renderers);
            
            foreach (var profile in _options.LogLevelProfiles.Values)
            {
                profile.OutputTemplate = "{Test}";
            }

            IRendererPipeline testInstance = new RendererPipeline(
                new OptionsWrapper<SpectreLoggerOptions>(_options),
                _rendererBuilder,
                _pool);
            
            testInstance.Render(new LogEventContext(
                "Logger",
                LogLevel.Information,
                default,
                "Test",
                null,
                Substitute.For<IScopeValues>(),
                _options.LogLevelProfiles[LogLevel.Information]));
        }
        
        [Fact]
        public void RenderInvokesPipeline()
        {
            foreach (var renderer in _renderers)
            {
                renderer.Received().Render(
                    Arg.Any<IWriteBuffer>(), 
                    Arg.Is<LogEventContext>(le =>
                        le.CategoryName == "Logger" &&
                        le.LogLevel == LogLevel.Information &&
                        le.State!.Equals("Test") &&
                        ReferenceEquals(le.Profile, _options.LogLevelProfiles[LogLevel.Information])));
            }
        }

        [Fact]
        public void RenderGetsAndReturnsWriteBuffer()
        {
            _pool.Received().Get();
            _pool.Received().Return(_writeBuffer);
        }

        [Fact]
        public void RenderResetsMargin()
        {
            _writeBuffer.Received().Margin = 0;
        }
    }
}