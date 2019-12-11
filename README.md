![Qowaiv JSON](https://github.com/Qowaiv/Qowaiv/blob/master/design/qowaiv-logo_linkedin_100x060.jpg)

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![Code of Conduct](https://img.shields.io/badge/%E2%9D%A4-code%20of%20conduct-blue.svg?style=flat)](https://github.com/Qowaiv/qowaiv-json/blob/master/CODE_OF_CONDUCT.md)

| version                                                                   | package                                                                                        |
|---------------------------------------------------------------------------|------------------------------------------------------------------------------------------------|
|![v](https://img.shields.io/badge/version-4.0.0-blue.svg?cacheSeconds=3600)|[Qowaiv.Json.Newtonsoft](https://www.nuget.org/packages/Qowaiv.Json.Newtonsoft/)                |
|![v](https://img.shields.io/badge/version-4.0.0-blue.svg?cacheSeconds=3600)|[Qowaiv.Text.Json.Serialization](https://www.nuget.org/packages/Qowaiv.Text.Json.Serialization/)|

# Qowaiv JSON
Serializing data using JSON is de facto the default. However, .NET has no
generic interface in the standard library to implement.

The solution provided here, is a (naming) convention based one, to serialize
Single Value Objects: Value Objects that can be represented by a single scalar.

1. There should be a public static factory method `FromJson(string)` returning 
   a new instance of the Single Value Object (SVO).
2. Optional factory methods for `double`, `long`, and `bool` can be provided.
   If not, the `string` factory method is used.
3. If a none `void` method `ToJson()` is provided, this one is used for
   serialization, otherwise `object.ToString()`.

## Sample Single Value Object

``` C#
public struct /* or class */ SingleValueObject
{
    // Required.
    public static SingleValueObject FromJson(string json);

    // Optional, otherwise FromJson(json.ToString(CultureInfo.Invariant)) is called.
    public static SingleValueObject FromJson(double json);

    // Optional, otherwise FromJson(json.ToString(CultureInfo.Invariant)) is called.
    public static SingleValueObject FromJson(long json);

    // Optional, otherwise FromJson(json ? "true": "false") is called.
    public static SingleValueObject FromJson(bool json);

    // Optional, otherwise ToString()
    public object /* or string, bool, int, long, double, decimal */ToJson();
}
```

## [System.Text.Json](https://docs.microsoft.com/en-us/dotnet/api/system.text.json)
Since .NET Core 3.0, Microsoft provides a built-in JSON serialization. To use
the `Qowaiv.Text.Json.Serialization` package the following code can be used:

``` C#
using Qowaiv.Json.Newtonsoft;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddMvc()
            .AddJsonOptions(options => 
            {
                options.JsonSerializerOptions.Converters.Add(new QowaivJsonConverter());
            });
    }
}
```

## [Newtonsoft](https://www.newtonsoft.com/json)
Newtonsoft's converter was the .NET de facto default, until .NET Core 3.0. To use
the `Qowaiv.Json.Newtonsoft` package the following code can be used:

``` C#
QowaivJsonConverter.Register();
```

Or, if you work with .NET core Web API:

``` C#
using Qowaiv.Text.Json.Serialization;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddMvc()
            .AddJsonOptions(options => 
            {
                options.SerializerSettings.Converters.Add(new QowaivJsonConverter());
            });
    }
}
```
