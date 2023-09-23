using NUnit.Framework;
using System;
using System.Collections.Generic;
using Reach;
using Reach.Rest.Api.Messaging;
using System.Linq;
namespace Reach.Tests 
{
    [TestFixture]
    class ClusterTest 
    {
        private string   apiUser;
        private string  apiKey;
        string  toNumber;
        string  fromNumber;
        [SetUp]
        [Category("ClusterTest")]
        public void SetUp()
        {
            apiUser = Environment.GetEnvironmentVariable("REACH_TALKYLABS_API_USER");
            apiKey = Environment.GetEnvironmentVariable("REACH_TALKYLABS_API_KEY");
            toNumber = Environment.GetEnvironmentVariable("REACH_TALKYLABS_TO_NUMBER");
            fromNumber = Environment.GetEnvironmentVariable("REACH_TALKYLABS_FROM_NUMBER");
            ReachClient.Init(username:apiUser,password:apiKey);
        }
        

        [Test]
        [Category("ClusterTest")]
        public void TestSendingAText()
        {
             var message = MessagingItemResource.Send(
                src: fromNumber,
                body: "Where's Wallace?",
                dest: toNumber
            );
            Assert.IsNotNull(message);
            Assert.True(message.Body.Contains("Where's Wallace?"));
            Assert.AreEqual(fromNumber,message.Src.ToString());
            Assert.AreEqual(toNumber,message.Dest.ToString());
        }

    }
}