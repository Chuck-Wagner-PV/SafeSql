using System;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Dapper;
using SafeSql.Core;
using Xunit;
using Xunit.Sdk;

namespace SafeSql.Tests
{
    public class SqlStatementTests
    {
        private static readonly SqlPhrase selectTop = new SqlPhrase("Select top");

        [Theory]
        [InlineData("john@gmail.com", "Select top 1 from users where type = 'CUSTOMER' AND role= 17 AND username = 'john@gmail.com'")]
        [InlineData("e91c7002-63b4-4c20-b2e8-0c43f5b9aa7a", "Select top 1 from users where type = 'CUSTOMER' AND role= 17 AND username = 'e91c7002-63b4-4c20-b2e8-0c43f5b9aa7a'")]
        [InlineData("attack' or 1=1 --", "Select top 1 from users where type = 'CUSTOMER' AND role= 17 AND username = 'attack'' or 1=1 --'")]
        [InlineData("㐀㑻", "Select top 1 from users where type = 'CUSTOMER' AND role= 17 AND username = N'㐀㑻'")]
        [InlineData(null, "Select top 1 from users where type = 'CUSTOMER' AND role= 17 AND username IS NULL")]
        public void ValidSqlStatement(string username, string expected)
        {
            var x = selectTop
                + 1 + new SqlPhrase("from users where type = ")
                    + "CUSTOMER".SqlStringConst()
                    + new SqlPhrase("AND role=")
                    + 17
                    + new SqlPhrase("AND username")
                    + SqlComparison.Equal.For(username)
                    + username;

            string result = null;
            x.Emit( val => result = val);
            Assert.Equal(expected, result);
        }




        [Theory]
        [InlineData("john@gmail.com")]
        [InlineData("e91c7002-63b4-4c20-b2e8-0c43f5b9aa7a")]
        [InlineData("attack' or 1=1 --")]
        [InlineData("㐀㑻")]
        [InlineData(null)]
        public void ValidSqlParameterized(string username)
        {
            var x = new SqlPhrase("Select top 1 from users where type = ")
                    + "CUSTOMER".SqlStringConst()
                    + new SqlPhrase("AND role=")
                    + 17
                    + new SqlPhrase("AND username")
                    + SqlComparison.Equal.For(username)
                    + username;

            string actualSql = null;

            DynamicParameters actualParams = null;
            x.EmitParameterized((val, parms) =>
                    {
                        actualSql = val;
                        actualParams = parms;
                    });

            var expectedSql = username != null
                ? "Select top 1 from users where type = @p0 AND role= @p1 AND username = @p2"
                : "Select top 1 from users where type = @p0 AND role= @p1 AND username IS @p2";

            Assert.Equal(expectedSql, actualSql);

            Assert.Equal("CUSTOMER", actualParams.Get<string>("p0"));
            Assert.Equal(17, actualParams.Get<int>("p1"));
            Assert.Equal(username, actualParams.Get<string>("p2"));
        }

    }
}
