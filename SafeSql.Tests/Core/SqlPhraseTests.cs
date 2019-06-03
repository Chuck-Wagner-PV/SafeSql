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
        [Theory]
        [InlineData("SELECT TOP 100 FIRSTNAME, [LASTNAME] FROM [USERS] ORDER BY LASTNAME")]

        // ensure that column names with T-SQL brackets work
        [InlineData(" ORDER BY [LASTNAME]")]
        public void ValidSql(string s)
        {
            var t = new SqlPhrase(s);
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
            Assert.Throws<SqlInjectionException>(() => new SqlPhrase(s));
        }

    }
}
