using System;
using System.Collections.Specialized;
using GridMvc.Utility;
using NUnit.Framework;
using PowerAssert;

namespace GridMvc.Tests.Utility
{
    [TestFixture]
    public class QueryStringBuilderTests
    {
        private CustomQueryStringBuilder _builder;

        [SetUp]
        public void Init()
        {

        }

        [Test]
        public void TestExcept()
        {
            var queryStringParameters = new NameValueCollection();
            queryStringParameters.Add("key1", "value1");
            queryStringParameters.Add("key2", "value2");
            _builder = new CustomQueryStringBuilder(queryStringParameters);

            var str1 = _builder.GetQueryStringExcept(new[] { "key1" });
            PAssert.IsTrue(() => str1 == "?key2=value2");

            str1 = _builder.GetQueryStringExcept(new[] { "key2" });
            PAssert.IsTrue(() => str1 == "?key1=value1");

            str1 = _builder.GetQueryStringExcept(new[] { "key1", "key2" });
            PAssert.IsTrue(() => str1 == "");
        }

        [Test]
        public void TestWithParameter()
        {
            var queryStringParameters = new NameValueCollection();
            queryStringParameters.Add("key1", "value1");
            queryStringParameters.Add("key2", "value2");
            queryStringParameters.Add("key3", "value3");

            _builder = new CustomQueryStringBuilder(queryStringParameters);

            var str1 = _builder.GetQueryStringWithParameter("key4", "value4");
            PAssert.IsTrue(() => str1 == "?key1=value1&key2=value2&key3=value3&key4=value4");

            str1 = _builder.GetQueryStringWithParameter("key4", "value4new");
            PAssert.IsTrue(() => str1 == "?key1=value1&key2=value2&key3=value3&key4=value4new");
        }
    }
}
