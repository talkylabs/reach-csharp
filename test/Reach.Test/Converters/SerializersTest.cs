using System;
using System.Collections.Generic;
using NUnit.Framework;
using Reach.Converters;

namespace Reach.Tests.Converters
{
    [TestFixture]
    public class SerializersTest : ReachTest {

        [Test]
        public void TestJsonObjectSerializesDictionary()
        {
            var inputDict = new Dictionary<string, string> {{"reach", "rocks"}};
            var result = Serializers.JsonObject(inputDict);
            Assert.AreEqual("{\"reach\":\"rocks\"}", result);
        }

        [Test]
        public void TestJsonObjectSerializesList()
        {
            var inputDict = new List<object>{
                "reach",
                new Dictionary<string, string> {{"join", "us"}}
            };
            var result = Serializers.JsonObject(inputDict);
            Assert.AreEqual("[\"reach\",{\"join\":\"us\"}]", result);
        }

        [Test]
        public void TestJsonObjectSerializesArray()
        {
            string[] inputDict = new string[2] {"reach", "rocks"};
            var result = Serializers.JsonObject(inputDict);
            Assert.AreEqual("[\"reach\",\"rocks\"]", result);
        }

        [Test]
        public void TestJsonObjectPassesThroughString()
        {
            var input = "{\"reach\":\"is dope\"}";
            var result = Serializers.JsonObject(input);
            Assert.AreEqual(input, result);
        }

        [Test]
        public void TestDateTimeIso8601WithDateTime()
        {
            var expect = "2017-06-19T12:13:14Z";
            var input = new DateTime(2017, 06, 19, 12, 13, 14);
            var result = Serializers.DateTimeIso8601(input);
            Assert.AreEqual(expect, result);
        }

        [Test]
        public void TestDateTimeIso8601WithNull()
        {
            var result = Serializers.DateTimeIso8601(null);
            Assert.AreEqual(null, result);
        }
    }
}
