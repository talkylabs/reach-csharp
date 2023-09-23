# reach-csharp


## Reach REST API for .NET

Reach provides a HTTP-based API for sending text messages. Learn more on [reach.talkylabs.com][apidocs].

More documentation for this library can be found [here][libdocs].

## Versions

`reach-csharp` uses a modified version of [Semantic Versioning](https://semver.org) for all changes. [See this document](VERSIONS.md) for details.

### TLS 1.2 Requirements

It is required to use TLS 1.2 when accessing the REST API. "Upgrade Required" errors indicate that TLS 1.0/1.1 is being used. With .NET, you can enable TLS 1.2 using this setting:

```csharp
System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
```

### Supported .NET versions

This library supports .NET applications that utilize .NET Framework 3.5+ or .NET Core 1.0+ (.NET Standard 1.4).

## Installation

The best and easiest way to add the Reach libraries to your .NET project is to use the NuGet package manager.

### With Visual Studio IDE

From within Visual Studio, you can use the NuGet GUI to search for and install the Reach-TalkyLabs NuGet package. Or, as a shortcut, simply type the following command into the Package Manager Console:

```shell
Install-Package Reach-TalkyLabs
```

### With .NET Core Command Line Tools

If you are building with the .NET Core command line tools, then you can run the following command from within your project directory:

```shell
dotnet add package Reach-TalkyLabs
```

## Sample usage

The examples below show how to have your application send an SMS message using the Reach .NET helper library:

```csharp
ReachClient.Init("API_USER", "API_KEY");

var message = MessagingItemResource.Send(
    src: "+11234567890",
    dest: "+10987654321",
    body: "Hello World!"
);
Console.WriteLine(message);
```

## Enable debug logging

There are two ways to enable debug logging in the default HTTP client. You can create an environment variable called `REACH_TALKYLABS_LOG_LEVEL` and set it to `debug` or you can set the LogLevel variable on the client as debug:

```csharp
ReachClient.SetLogLevel("debug");
```

## Handle exceptions

For an example on how to handle exceptions in this helper library, please see the Reach documentation.


## Use a custom HTTP Client

To use a custom HTTP client with this helper library, please see the [advanced example of how to do so](./advanced-examples/custom-http-client.md).

## Docker Image

The `Dockerfile` present in this repository and its respective `talkylabs/reach-csharp` Docker image are used by Talkylabs for testing purposes.

You could use the docker image for building and running tests:

```bash
docker build -t reachbuild .
```

Bash:

```bash
docker run -it --rm -v $(pwd):/talkylabs reachbuild
make test
```

Powershell:

```pwsh
docker run -it --rm -v ${PWD}:/talkylabs reachbuild
make test
```

## Get support

If you've found a bug in the library or would like new features added, go ahead and open issues or pull requests against this repo!

[reach]: https://www.reach.talkylabs.com
[apidocs]: https://www.reach.talkylabs.com/docs/api
[libdocs]: https://talkylabs.github.io/reach-csharp