﻿using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VerifyXunit;
using Vertical.SpectreLogger.Tests.Infrastructure;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Rendering
{
    [UsesVerify]
    public class MarginControlRendererTests
    {
        [Fact]
        public Task RenderWritesExpectedOutputForSet()
        {
            var output = RendererTestHarness.Capture(
                config => config.ConfigureProfile(LogLevel.Information, profile =>
                {
                    profile.OutputTemplate = "Indent-0{Margin=5}{NewLine}Indent-5{Margin=0}{NewLine}Indent-0";
                }),
                logger => logger.LogInformation(""));

            return Verifier.Verify(output);
        }

        [Fact]
        public Task RenderWritesExpectedOutputForAdjust()
        {
            var output = RendererTestHarness.Capture(
                config => config.ConfigureProfile(LogLevel.Information, profile => profile
                    .OutputTemplate = "Indent-0{Margin+2}{NewLine}Indent+2{Margin-2}{NewLine}Indent-0"),
                logger => logger.LogInformation(""));

            return Verifier.Verify(output);  
        }
    }
}
