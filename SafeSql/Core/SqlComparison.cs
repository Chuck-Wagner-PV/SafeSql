namespace SafeSql.Core
{
    /// <summary> Generates the appropriate SQL operator for a string value.</summary>
    public class SqlComparison
    {
        public static readonly SqlComparison Equal = new SqlComparison(SqlOperator.Equal, SqlOperator.Is);
        public static readonly SqlComparison NotEqual = new SqlComparison(SqlOperator.NotEqual, SqlOperator.IsNot);

        private readonly SqlFragment _valueOperator;
        private readonly SqlFragment _nullOperator;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Private constructor. </summary>
        ///
        /// <param name="valueOperator">    The operator to use if the string has a value. </param>
        /// <param name="nullOperator">     The operator to use if the string is null. </param>
        private SqlComparison(SqlFragment valueOperator, SqlFragment nullOperator)
        {
            this._valueOperator = valueOperator;
            this._nullOperator = nullOperator;
        }

        public SqlFragment For(string value)
        {
            return value == null ? _nullOperator : _valueOperator;
        }

    }
}