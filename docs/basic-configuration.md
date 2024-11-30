# Basic Configuration

The logging provider will set a reasonable set of defaults out-of-box, so you are not required to configure anything to get up and running. Listed below are some basic tips to help you on your customization journey.

## Configuration Overview

Configuration is divided into two main parts:

1. The main `SpectreLoggerOptions` object that controls various global settings of the logger.
2. A collection of `LogLevelProfile` option objects that control the customization to events of a particular log event severity (e.g. Debug, Info, etc.).

### Global Options

All configuration is done during the setup phase and is accessible when calling the `AddSpectreConsole` extension method on the `ILoggingBuilder` interface. The method accepts a configuration delegate that provides a `SpectreLoggerBuilder` object that can be manipulated and is shown in the following example:

```csharp
var loggerFactory = LoggerFactory
    .Create(buidler => 
    {
        builder.AddSpectreConsole(config =>
        {
            // TODO: Customize the logger here
        });
    });
```

> 💡 Note
> 
> In all of the examples hereafter, assume the `config` object is a `SpectreLoggerBuilder` instance.

### Setting the minimum log event level.

The provider will not display any log events introduced that are of less of a severity than the configured value. By default, the provider sets the minimum level to `LogLevel.Information`. Additionally, you can configure the minimum log level for specific or groups of logging categories. Filters are selected using the best substring category match.

```csharp
// Trace & debug events are not displayed for any events

config.SetMinimumLevel(LogLevel.Information);

// Override minimum levels for specific logging categories

config.SetMinimumLevel("System", LogLevel.Warning);
config.SetMinimumLevel("System.Net.HttpClient", LogLevel.Warning);
```


### Filtering log events using a service.

If a filtering service is provided, the current log event is evaluated before being displayed. You can provide your own implementation of `ILogEventFilter` to handle this function.

```csharp
public class MyLogEventFilter : ILogEventFilter
{
     public bool Filter(in LogEventContext eventContext)
     {
        if (eventContext.EventId.Id == 100)
        {
            // Don't display
            return false;
        }
        
        // Don't filter
        return true;
     }
}
```

For more simple scenarios, filtering can be handled by a delegate.

```csharp
config.SetLogEventFilter((in LogEventContext context) => context.EventId.Id == 100);
```

> 💡 Note
> 
> It is recommended that you use these log filtering events to customize output particular to console logging. Otherwise, consider using the default filtering mechanisms available in the Microsoft logging implementation to ensure consistent policies across your application's logging infrastructure.

### Configuring log level profiles

Log level profiles allow you to customize output based on the event severity. That means that `Warning` events can have different styles, formatting, and output templates than that of `Debug` events. Settings can be applied to all profiles at one time, or to individual profiles.

Log level profiles are discussed in great detail in specific renderer documentation, but the jist is:

```csharp
// Configure the profile of a single level
config.ConfigureProfile(LogLevel.Information, profile => 
{
    // TODO: Configure
});
    
// Configure multiple profiles with the same settings
config.ConfigureProfiles(new[]{LogLevel.Trace, LogLevel.Debug}, profile =>
{
    // TODO: Configure
});
    
// Configure all profiles with the same settings
optiosn.ConfigureProfiles(profile =>
{
    // TODO: Configure
});
```

> ☑️ Tip
> 
> Configure profiles from the least specific to the most specific. Keep in mind you can overwrite settings made in previous configuration calls.

## See Also

- [(Next) Output Templates](./output-template.md)
- [Advanced Configuration](./advanced-config.md)
- [Rendering Overview](./renderer-overview.md)
