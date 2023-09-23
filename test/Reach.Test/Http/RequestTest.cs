using NUnit.Framework;
using System;
using Reach.Http;
using Reach.Rest;

namespace Reach.Tests.Http
{
    [TestFixture]
    public class RequestTest
    {
        [Test]
        public void TestNoEdgeOrRegionInUrl()
        {
            var request = new Request(HttpMethod.Get, "https://api.reach.talkylabs.com");

            Assert.AreEqual(new Uri("https://api.reach.talkylabs.com"), request.buildUri());
        }

        [Test]
        public void TestRegionAndEdgeInConstrcutor()
        {
            var request = new Request(HttpMethod.Get, Domain.Api, "/path/to/something.json?foo=12.34");

            Assert.AreEqual(new Uri("https://api.reach.talkylabs.com/path/to/something.json?foo=12.34"), request.buildUri());
        }
    }
}
