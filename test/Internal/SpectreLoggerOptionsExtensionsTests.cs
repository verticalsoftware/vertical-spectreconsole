using Microsoft.Extensions.Logging;
using Shouldly;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Options;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Internal;

public class SpectreLoggerOptionsExtensionsTests
{
    private class MyTheoryData : TheoryData<string, LogLevel, LogLevel, string, LogLevel>
    {
        public MyTheoryData()
        {
            // No match - should always be base level
            AddTheory(
                overrideCategory: "MyApp",
                overrideLevel: LogLevel.Information,
                baseLevel: LogLevel.Information,
                loggerCategory: "Microsoft",
                expected: LogLevel.Information);

            AddTheory(
                overrideCategory: "MyApp",
                overrideLevel: LogLevel.Debug,
                baseLevel: LogLevel.Information,
                loggerCategory: "Microsoft",
                expected: LogLevel.Information);

            // Full match, but less than base level (should be base level)
            AddTheory(
                overrideCategory: "Microsoft",
                overrideLevel: LogLevel.Debug,
                baseLevel: LogLevel.Information,
                loggerCategory: "Microsoft",
                expected: LogLevel.Information);

            // Full match, but equal to base level (should be base level)
            AddTheory(
                overrideCategory: "Microsoft",
                overrideLevel: LogLevel.Information,
                baseLevel: LogLevel.Information,
                loggerCategory: "Microsoft",
                expected: LogLevel.Information);

            // Full match, but greater than base level (should be override level)
            AddTheory(
                overrideCategory: "Microsoft",
                overrideLevel: LogLevel.Warning,
                baseLevel: LogLevel.Information,
                loggerCategory: "Microsoft",
                expected: LogLevel.Warning);

            // Partial match, but less than base level (should be base level)
            AddTheory(
                overrideCategory: "Microsoft",
                overrideLevel: LogLevel.Debug,
                baseLevel: LogLevel.Information,
                loggerCategory: "Microsoft.Hosting",
                expected: LogLevel.Information);

            // Partial match, but equal to base level (should be base level)
            AddTheory(
                overrideCategory: "Microsoft",
                overrideLevel: LogLevel.Information,
                baseLevel: LogLevel.Information,
                loggerCategory: "Microsoft.Hosting",
                expected: LogLevel.Information);

            // Partial match, but greater than base level (should be override level)
            AddTheory(
                overrideCategory: "Microsoft",
                overrideLevel: LogLevel.Warning,
                baseLevel: LogLevel.Information,
                loggerCategory: "Microsoft.Hosting",
                expected: LogLevel.Warning);
        }

        private void AddTheory(string overrideCategory,
            LogLevel overrideLevel,
            LogLevel baseLevel,
            string loggerCategory,
            LogLevel expected)
        {
            Add(overrideCategory, overrideLevel, baseLevel, loggerCategory, expected);
        }
    }
    
    [Theory, ClassData(typeof(MyTheoryData))]
    public void GetLogLevelFilter_Returns_Expected(
        string overrideCategory,
        LogLevel overrideLevel,
        LogLevel baseLevel,
        string loggerCategory,
        LogLevel expected)
    {
        var unit = new SpectreLoggerOptions
        {
            MinimumLevelOverrides =
            {
                [overrideCategory] = overrideLevel
            },
            MinimumLogLevel = baseLevel
        };

        var result = unit.GetLogLevelFilter(loggerCategory);
        result.ShouldBe(expected);
    }

    [Fact]
    public void GetLogLevelFilter_Selects_Best_Match()
    {
        var unit = new SpectreLoggerOptions
        {
            MinimumLevelOverrides =
            {
                ["Microsoft.Hosting"] = LogLevel.Warning,
                ["Microsoft.Hosting.Internal"] = LogLevel.Error
            }
        };
        
        unit.GetLogLevelFilter("Microsoft.Hosting").ShouldBe(LogLevel.Warning);
        unit.GetLogLevelFilter("Microsoft.Hosting.Internal").ShouldBe(LogLevel.Error);
        unit.GetLogLevelFilter("Microsoft.Hosting.Internal.AspNetCore").ShouldBe(LogLevel.Error);
    }
}