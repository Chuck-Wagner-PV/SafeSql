namespace SafeSql.Core.Parameters
{
    /// <summary>   Used for quoted strings from user or database values,
    ///             will return an unquoted NULL if passed one. </summary>
    public class StringValueParam : SqlFragment, ISqlParameter
    {
        //private const string quotesPattern = @"['\u02BC]";
        //private static readonly Regex quotes = new Regex(quotesPattern, RegexOptions.Singleline);

        public StringValueParam(string stringValue) : base(stringValue)
        {
        }

        public override string Escape(string value)
        {
            if (value == null)
            {
                return "NULL";
            }

            // prevent sql injection by escaping the quotes.
            var escaped = value.Replace("'", "''");

            // prevent sql smuggling by detect non-ascii characters and forcing the quoted string to be treated as unicode.
            foreach (var c in escaped)
            {
                if (c > sbyte.MaxValue)
                {
                    return $"N'{escaped}'";
                }
            }

            return $"'{escaped}'";
        }

        public override bool IsValid(string stringValue)
        {
            // consider adding pattern detection for well known attacks and refusing the value.
            return true;
        }

        public object ParameterValue => Value;

    }
}