using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using VerifyXunit;
using Vertical.SpectreLogger.Destructuring;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Tests.Infrastructure;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Destructuring
{
    public class DestructuringWriterTests
    {
        [Fact]
        public Task WriteDictionaryRendersExpectedContent()
        {
            var profile = new LogLevelProfile(LogLevel.Information);
            var buffer = new WriteBuffer(Substitute.For<IConsoleWriter>());
            var dictionary = new Dictionary<string, object>
            {
                ["firstName"] = "Testy",
                ["lastName"] = "McTesterson",
                ["address"] = new Dictionary<string, string>
                {
                    ["street"] = "123 Main Street",
                    ["city"] = "Denver",
                    ["state"] = "CO"
                }
            };
            
            DestructuringWriter.Write(buffer, profile, dictionary);
            
            return Verifier.Verify(buffer.ToString());
        }

        [Fact]
        public Task WriteObjectRendersExpectedContent()
        {
            var obj = new
            {
                FirstName = "Testy",
                LastName = "McTesterson",
                Address = new
                {
                    Street = "123 Main Street",
                    City = "Denver",
                    State = "CO"
                }
            };

            var buffer = new WriteBuffer(Substitute.For<IConsoleWriter>());
            var profile = new LogLevelProfile(LogLevel.Information);
            
            DestructuringWriter.Write(buffer, profile, obj);
            
            return Verifier.Verify(buffer.ToString());
        }

        [Fact]
        public void WriteArrayRendersExpectedContent()
        {
            var obj = new[] {"one", "two", "three"};

            var buffer = new WriteBuffer(Substitute.For<IConsoleWriter>());
            var profile = new LogLevelProfile(LogLevel.Information);
            
            DestructuringWriter.Write(buffer, profile, obj);
            
            buffer.ToString().ShouldBe("[[one, two, three]]");
        }

        [Fact]
        public void WriteEmptyArrayIndentedRendersExpectedContent()
        {
            var buffer = new WriteBuffer(Substitute.For<IConsoleWriter>());
            var profile = new LogLevelProfile(LogLevel.Information);
            profile.ConfigureOptions<DestructuringOptions>(opt => opt.WriteIndented = true);

            DestructuringWriter.Write(buffer, profile, Array.Empty<string>());

            buffer.ToString().ShouldBe("[[]]");
        }
        
        [Fact]
        public void WriteEmptyDictionaryIndentedRendersExpectedContent()
        {
            var buffer = new WriteBuffer(Substitute.For<IConsoleWriter>());
            var profile = new LogLevelProfile(LogLevel.Information);
            profile.ConfigureOptions<DestructuringOptions>(opt => opt.WriteIndented = true);

            DestructuringWriter.Write(buffer, profile, new Dictionary<string, string>());

            buffer.ToString().ShouldBe("{}");
        }

        [Fact]
        public Task WriteDictionaryRespectsMaxDepth()
        {
            var obj = new Person(
                "Testy",
                new[]
                {
                    new Person(
                        "Testy Jr.",
                        new[]
                        {
                            new Person(
                                "Testy III",
                                new[]
                                {
                                    new Person("Testy IV",
                                        Array.Empty<Person>())
                                })
                        })
                });

            var profile = new LogLevelProfile(LogLevel.Information);
            profile.ConfigureOptions<DestructuringOptions>(opt => opt.MaxDepth = 3);

            var buffer = new WriteBuffer(Substitute.For<IConsoleWriter>());
            DestructuringWriter.Write(buffer, profile, obj);

            return Verifier.Verify(buffer.ToString());
        }

        [Fact]
        public Task WriteObjectIndented()
        {
            var obj = new
            {
                Id = Guid.Parse("5e093875-2c3f-4d28-9718-1339aa03f9ca"),
                FirstName = "Testy",
                LastName = "McTesterson",
                Address = new
                {
                    Street = "6715 W Colfax Ave",
                    City = "Lakewood",
                    State = "CO",
                    ZipCode = "80214"
                },
                Roles = new[] { "Manager", "Cook" }
            };

            var profile = new LogLevelProfile(LogLevel.Information);
            profile.ConfigureOptions<DestructuringOptions>(opt => opt.WriteIndented = true);

            var buffer = new WriteBuffer(Substitute.For<IConsoleWriter>());
            DestructuringWriter.Write(buffer, profile, obj);

            return Verifier.Verify(buffer.ToString());
        }
    }
}