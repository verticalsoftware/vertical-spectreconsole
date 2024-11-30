using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using Shouldly;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Tests
{
    public static class TestHelpers
    {
        public static void ShouldHaveElementsEqualTo<T>(
            this IEnumerable<T> source, 
            IEnumerable<T> expected,
            Action<T, T> comparer)
        {
            var queue = new Queue<T>(expected);

            foreach (var item in source)
            {
                queue.Any().ShouldBeTrue();

                var nextExpected = queue.Dequeue();

                comparer(item, nextExpected!);
            }

            queue.ShouldBeEmpty($"{queue.Count} position control(s) not matched.");
        }

        public static void VerifyOutput(
            this ITemplateRenderer renderer,
            in LogEventContext eventContext,
            string expectedContent)
        {
            var buffer = new WriteBuffer(Substitute.For<IConsoleWriter>());
            
            renderer.Render(buffer, eventContext);
            
            buffer.ToString().ShouldBe(expectedContent);
        }
    }
}