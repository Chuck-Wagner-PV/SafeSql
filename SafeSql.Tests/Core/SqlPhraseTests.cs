using System;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using SafeSql.Core;
using Xunit;
using Xunit.Sdk;

namespace SafeSql.Tests
{
    public class SqlPhraseTests
    {
        [Fact]
        public void ValidSelect()
        {
            var t = new SqlPhrase("SELECT TOP 100 FIRSTNAME, [LASTNAME] FROM [USERS] ORDER BY LASTNAME");
        }

        [Theory]
        // Hardcoded string parameters not allowed
        [InlineData("SELECT FIRSTNAME, LASTNAME FROM USERS WHERE USERNAME = 'JOHN'")]
        // end of line comment
        [InlineData("SELECT FIRSTNAME, LASTNAME FROM USERS WHERE ID = 1234\n OR 1=1 --")]
        // block comment
        [InlineData("SELECT FIRSTNAME, LASTNAME FROM USERS WHERE ID = 1234 OR 1=1 /*")]

        public void SqlInjectionCaught(string s)
        {
            Assert.Throws<SqlInjectionException>(() => (SqlPhrase)s);
        }

        [Theory]
        [InlineData("123")]
        public void PhraseMustBeACompileTimeConstant(string customerId)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                return new SqlPhrase("Select top 1 from customers where customerid = " + customerId);
            });
        }
    }
}
