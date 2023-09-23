using System;
using System.Net;
using System.Collections.Generic;

#if !NET35
using System.Threading.Tasks;
#endif

using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Reach.Clients;
using Reach.Rest;
using Reach.Exceptions;
using Reach.Http;
using System.IO;
using Reach.Rest.Api.Authentix.ConfigurationItem;

namespace Reach.Tests.Clients
{
    [TestFixture]
    public class ReachRestClientTest
    {
        HttpClient client;

        string uri = "/v1/fetch?messageId=MM123";
        string authResponse = "{" +
    		"  \"appletId\": \"AIDXXXXXXXXXXXX\"," +
    		"  \"apiVersion\": \"1.0.0\"," +
    		"  \"configurationId\": \"CIDXXXXXXXXXXXX\"," +
    		"  \"authenticationId\": \"VIDXXXXXXXXXXXX\"," +
    		"  \"status\": \"awaiting\"," +
    		"  \"dest\": \"+237671234567\"," +
    		"  \"channel\": \"sms\"," +
    		"  \"expiryTime\": 5," +
    		"  \"maxTrials\": 5," +
    		"  \"maxControls\": 3," +
    		"  \"paymentInfo\": {" +
    		"    \"payee\": \"ACME\"," +
    		"    \"amount\": 1000," +
    		"    \"currency\": \"xaf\"" +
    		"  }," +
    		"  \"trials\": [" +
    		"    {" +
    		"      \"dateCreated\": \"2016-08-29T09:12:33.001Z\"," +
    		"      \"trialId\": \"TRDXXXXXXXXXX\"," +
    		"      \"channel\": \"sms\"" +
    		"    }" +
    		"  ]," +
    		"  \"dateCreated\": \"2016-08-29T09:12:33.001Z\"," +
    		"  \"dateUpdated\": \"2016-08-29T09:12:35.001Z\"" +
    		"}";

        [SetUp]
        public void Init()
        {
            client = Substitute.For<HttpClient>();
        }

        

        [Test]
        public void TestDeserialization()
        {
            client.MakeRequest(Arg.Any<Request>()).Returns(new Response(HttpStatusCode.OK, authResponse));
            Request request = new Request(HttpMethod.Get, Domain.Api, uri);
            ReachRestClient reachClient = new ReachRestClient("foo", "bar", client);
            Response resp = reachClient.Request(request);
            Assert.IsNotNull(resp);
            AuthenticationItemResource item = AuthenticationItemResource.FromJson(resp.Content);
            Assert.IsNotNull(item);
        }
        

        [Test]
        public void TestBadResponseWithDetails()
        {
            string jsonResponse = @"{
                                    ""ErrorCode"": 20001,
                                    ""ErrorMessage"": ""Bad request"",
                                    ""more_info"": ""https://www.abc.com/docs/errors/20001"",
                                    ""status"": 400,
                                    ""ErrorDetails"": {
                                        ""foo"": ""bar""
                                    }}";
            client.MakeRequest(Arg.Any<Request>()).Returns(new Response(HttpStatusCode.BadRequest, jsonResponse));
            try
            {
                Request request = new Request(HttpMethod.Get, "https://www.contoso.com");
                ReachRestClient reachClient = new ReachRestClient("foo", "bar", client);
                reachClient.Request(request);
                Assert.Fail("Should have failed");
            }
            catch (ApiException e)
            {
                Assert.AreEqual("Bad request", e.Message);
                Assert.AreEqual(20001, e.Code);
                Assert.AreEqual("https://www.abc.com/docs/errors/20001", e.MoreInfo);
                Assert.AreEqual(400, e.Status);
                var expectedDetails = new Dictionary<string, object>();
                expectedDetails.Add("foo", "bar");
                Assert.AreEqual(expectedDetails, e.Details);
            }
        }

        [Test]
        public void TestRedirectResponse()
        {
            client.MakeRequest(Arg.Any<Request>()).Returns(new Response(HttpStatusCode.RedirectKeepVerb, "REDIRECT"));
            Request request = new Request(HttpMethod.Get, "https://www.contoso.com");
            ReachRestClient reachClient = new ReachRestClient("foo", "bar", client);
            reachClient.Request(request);
        }

        [Test]
        public void TestActivatingDebugLogging()
        {
            var output = new StringWriter();
            Console.SetOut(output);
            client.MakeRequest(Arg.Any<Request>()).Returns(new Response(HttpStatusCode.OK, "OK"));
            Request request = new Request(HttpMethod.Get, "https://www.contoso.com");
            ReachRestClient reachClient = new ReachRestClient("foo", "bar", client);
            reachClient.LogLevel = "debug";
            reachClient.Request(request);
            Assert.That(output.ToString(), Contains.Substring("request.URI: https://www.contoso.com/"));
        }

        [Test]
        public void RequestWithUserAgentExtensions()
        {
            client.MakeRequest(Arg.Any<Request>()).Returns(new Response(HttpStatusCode.OK, "OK"));
            Request request = new Request(HttpMethod.Get, "https://api.reach.talkylabs.com/");
            string[] userAgentExtensions = new string[] { "reach-run/2.0.0-test", "flex-plugin/3.4.0" };
            ReachRestClient reachClient = new ReachRestClient("foo", "bar", httpClient: client);
            reachClient.UserAgentExtensions = userAgentExtensions;

            reachClient.Request(request);

            Assert.AreEqual(request.UserAgentExtensions, userAgentExtensions);
        }

#if !NET35
        
        [Test]
        public async Task RequestAsyncWithUserAgentExtensions()
        {
            client.MakeRequestAsync(Arg.Any<Request>()).Returns(new Response(HttpStatusCode.OK, "OK"));
            Request request = new Request(HttpMethod.Get, "https://api.reach.talkylabs.com/");
            string[] userAgentExtensions = new string[] { "reach-run/2.0.0-test", "flex-plugin/3.4.0" };
            ReachRestClient reachClient = new ReachRestClient("foo", "bar", httpClient: client);
            reachClient.UserAgentExtensions = userAgentExtensions;

            await reachClient.RequestAsync(request);

            Assert.AreEqual(request.UserAgentExtensions, userAgentExtensions);
        }
#endif
    }
}
