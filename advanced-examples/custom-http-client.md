# Custom HTTP Clients for the Reach C# Helper Library with .NET Framework

If you are working with the Reach C# / .NET Helper Library, and you need to be able to modify the HTTP requests that the library makes to the Reach servers, you’re in the right place. The most common need to alter the HTTP request is to connect and authenticate with an enterprise’s proxy server. We’ll provide sample code that you can drop right into your app to handle this use case.

## Connect and authenticate with a proxy server

To connect and provide credentials to a proxy server that may be between your app and Reach, you need a way to modify the HTTP requests that the Reach helper library makes on your behalf to invoke the Reach REST API.

On .NET 4.5.1 and above, the Reach helper library uses the `HttpClient` class (in the `System.Net.Http` `namespace`) under the hood to make the HTTP requests. Knowing this, the following two facts should help us arrive at the solution:

- Connecting to a proxy server with `HttpClient` is a [solved problem](https://gist.github.com/bryanbarnard/8102915).
- The Reach Helper Library allows you to provide your own `HttpClient` for making API requests.

So the question becomes how do we apply this to a typical Reach REST API example?

```csharp
ReachClient.Init(api_user, api_key);

var message = MessagingItemResource.Send(
    dest: "+15558675309",
    src: "+15017250604",
    body: "Hello from C#");
```

Where does a `ReachRestClient` get created and used? Out of the box, the helper library is creating a default `ReachRestClient` for you, using the Reach credentials you pass to the `Init` method. However, there’s nothing stopping you from creating your own and using that.

Once you have your own `ReachRestClient`, you can pass it to any Reach REST API resource action you want. Here’s an example of sending an SMS message with a custom client:

```csharp
using System;
using Reach;
using Reach.Rest.Api.Messaging;
using Reach.Types;

namespace CustomClientDotNet4x
{
    class Program
    {
        static void Main(string[] args)
        {
            var reachRestClient = ProxiedReachClientCreator.GetClient();

            // Now that we have our custom built ReachRestClient,
            // we can pass it to any REST API resource action.
            var message = MessagingItemResource.Send(
                dest: "+15017122661",
                src: "+15017122661",
                body: "Hey there!",
                // Here's where you inject the custom client
                client: reachRestClient
            );

            Console.WriteLine($"Message ID: {message.MessageId}");
        }
    }
}
```

## Create your custom ReachRestClient

When you take a closer look at the constructor for `ReachRestClient`, you see that the `httpClient` parameter is actually of type `Reach.Http.HttpClient` and not the `System.Net.HttpClient` we were expecting. `Reach.Http.HttpClient` is actually an abstraction that allows plugging in any implementation of an HTTP client you want (or even creating a mocking layer for unit testing).

However, within the helper library, there is an implementation of `Reach.Http.HttpClient` called SystemNetHttpClient. This class wraps the `System.Net.HttpClient` and provides it to the Reach helper library to make the necessary HTTP requests.

## Call Reach through the proxy server

Now that we understand how all the components fit together, we can create our own `ReachRestClient` that can connect through a proxy server. To make this reusable, here’s a class that you can use to create this `ReachRestClient` whenever you need one.

```csharp
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using Reach.Clients;

namespace CustomClientDotNet4x
{
    public static class ProxiedReachClientCreator
    {
        private static HttpClient _httpClient;

        private static void CreateHttpClient()
        {
            var proxyUrl = ConfigurationManager.AppSettings["ProxyServerUrl"];
            var handler = new HttpClientHandler()
            {
                Proxy = new WebProxy(proxyUrl),
                UseProxy = true
            };

            _httpClient = new HttpClient(handler);
            var byteArray = Encoding.Unicode.GetBytes(
                ConfigurationManager.AppSettings["ProxyUsername"]
                    + ":"
                    + ConfigurationManager.AppSettings["ProxyPassword"]
            );

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(byteArray)
                );
        }

        public static ReachRestClient GetClient()
        {
            var api_user = ConfigurationManager.AppSettings["ReachApiUser"];
            var api_key = ConfigurationManager.AppSettings["ReachApiKey"];

            if (_httpClient == null)
            {
                // It's best* to create a single HttpClient and reuse it
                // * See: https://goo.gl/FShAAe
                CreateHttpClient();
            }

            var reachRestClient = new ReachRestClient(
                api_user,
                api_key,
                httpClient: new Reach.Http.SystemNetHttpClient(_httpClient)
            );

            return reachRestClient;
        }
    }
}
```

Notice the use of `ConfigurationManager.AppSettings` to retrieve various configuration settings:

- Your Reach API user and key that are found in the web application.
- Your proxy server URL (including the server name or address and port number)
- Your username and password for the proxy server

These settings can be placed in your Web.config or App.config (for a console app) like so:

```xml
<appSettings>
  <!-- Find your Reach Api user and key -->
  <add key="ReachApiUser" value="XXxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" />
  <add key="ReachApiKey" value="your_api_key" />

  <!-- Replace the following with your proxy server's settings -->
  <add key="ProxyServerUrl" value="http://127.0.0.1:8888"/>
  <add key="ProxyUsername" value="your_username"/>
  <add key="ProxyPassword" value="your_password"/>
</appSettings>
```

Here’s a console program that sends a text message and shows how it all can work together.

```csharp
using System;
using Reach.Rest.Api.Messaging;
using Reach.Types;

namespace CustomClientDotNet4x
{
    class Program
    {
        static void Main(string[] args)
        {
            var reachRestClient = ProxiedReachClientCreator.GetClient();

            // Now that we have our custom built ReachRestClient,
            // we can pass it to any REST API resource action.
            var message = MessagingItemResource.Send(
                dest: "+15017122661",
                src: "+15017122661",
                body: "Hey there!",
                // Here's where you inject the custom client
                client: reachRestClient
            );

            Console.WriteLine($"Message Id: {message.MessageId}");
        }
    }
}
```

## What else can this technique be used for?

Now that you know how to inject your own System.Net.HttpClient into the Reach API request pipeline, you could use this technique to add custom HTTP headers to the requests (perhaps as required by an upstream proxy server).

You could also implement your own Reach.Http.HttpClient to mock the Reach API responses so your unit and integration tests can run quickly without the need to make a connection to Reach.

We can’t wait to see what you build!
