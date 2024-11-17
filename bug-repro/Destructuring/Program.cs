using System.Security.Cryptography;
using Destructuring;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger;
using Vertical.SpectreLogger.Options;

var loggerFactory = LoggerFactory.Create(builder => builder.AddSpectreConsole(
    sc => sc.ConfigureProfile(LogLevel.Information, profile => profile
        .ConfigureOptions<DestructuringOptions>(opt =>
        {
            opt.IndentSpaces = 2;
            opt.WriteIndented = true;
            opt.MaxProperties = 10;
        }))));

var logger = loggerFactory.CreateLogger("Program");

var model = new Model(
    Guid.NewGuid(),
    "1.0.0",
    DateTime.Now,
    Guid.NewGuid(),
    "/Users/me",
    "migration.00001.sql",
    RandomNumberGenerator.GetHexString(32, lowercase: true),
    "agent",
    "host",
    Metrics: new Dictionary<string, string>
    {
        ["db/rowsAffected"] = "10"
    },
    OperationTags: new Dictionary<string, string>
    {
        ["postgres/npgSqlVersion"] = "2.1.4",
        ["postgres/dapperVersion"] = "8.0.5"
    },
    Metadata: new Dictionary<string, string>
    {
        ["build"] = RandomNumberGenerator.GetHexString(8, lowercase: true),
        ["task"] = "DEV_2482",
        ["author"] = "dan"
    });
    
    logger.LogInformation("Info = {@model}", model);