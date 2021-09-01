using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Rendering;
using Vertical.SpectreLogger.Templates;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Internal
{
    public class TemplateRendererBuilderTests
    {
        private readonly TemplateRendererBuilder _testInstance = new(new List<RendererDescriptor>
        {
            new(typeof(NameRenderer)),
            new(typeof(AddressRenderer)),
            new(typeof(IdRenderer))
        });
        
        [Template("{name}")]
        public class NameRenderer : ITemplateRenderer
        {
            /// <inheritdoc />
            public void Render(IWriteBuffer buffer, in LogEventInfo logEventInfo)
            {
            }
        }

        [Template("{id}")]
        public class IdRenderer : ITemplateRenderer
        {
            public IdRenderer(TemplateContext context)
            {
            }
            
            /// <inheritdoc />
            public void Render(IWriteBuffer buffer, in LogEventInfo logEventInfo)
            {
            }
        }

        [Template("{address}")]
        public class AddressRenderer : ITemplateRenderer
        {
            /// <inheritdoc />
            public void Render(IWriteBuffer buffer, in LogEventInfo logEventInfo)
            {
            }
        }
        
        [Fact]
        public void GetRenderersReturnsExpectedItems()
        {
            var pipeline = _testInstance.GetRenderers("{name}{address}");

            pipeline.Count.ShouldBe(2);
            pipeline[0].ShouldBeOfType<NameRenderer>();
            pipeline[1].ShouldBeOfType<AddressRenderer>();
        }

        [Fact]
        public void GetRenderersReturnsStaticSpanRenderersInserted()
        {
            var pipeline = _testInstance.GetRenderers("my name is {name}!");

            pipeline.Count.ShouldBe(3);
            pipeline[0].ShouldBeOfType<StaticSpanRenderer>();
            pipeline[1].ShouldBeOfType<NameRenderer>();
            pipeline[2].ShouldBeOfType<StaticSpanRenderer>();
        }

        [Fact]
        public void GetRenderersShouldBuildWithTemplateContext()
        {
            var pipeline = _testInstance.GetRenderers("{id}");

            pipeline.Single().ShouldBeOfType<IdRenderer>();
        }

        [Fact]
        public void GetRenderersShouldReturnStaticSpanForUnmatchedTemplate()
        {
            var pipeline = _testInstance.GetRenderers("{unknown}");

            var renderer = pipeline.Single();
            
            renderer.ShouldBeOfType<StaticSpanRenderer>();
            renderer.VerifyOutput(default, "{unknown}");
        }
    }
}