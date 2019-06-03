using System;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using SafeSql.Core;
using SafeSql.Core.Parameters;
using Xunit;
using Xunit.Sdk;

namespace SafeSql.Tests
{
    public class StringConstParamTests
    {
        [Theory]
        [InlineData("DEBIT")]
        public void ValidStringConstant(string s)
        {
            var t = new StringConstParam(s);
        }

        [Theory]
        // Don't allow use of a constant as a value
        [InlineData("JOHN' OR 1=1")]

        public void SqlInjectionCaught(string s)
        {
            Assert.Throws<SqlInjectionException>(() => new StringConstParam(s));
        }

    }
}
