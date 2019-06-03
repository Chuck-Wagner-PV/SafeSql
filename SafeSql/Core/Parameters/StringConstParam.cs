using System.Text.RegularExpressions;

namespace SafeSql.Core.Parameters
{
    /// <summary>   Used for quoted string constants, eg. 'DEBIT' </summary>
    public class StringConstParam : SqlFragment, ISqlParameter
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   The pattern for a string constant. </summary>
        ///
        /// <remarks>
        /// DO NOT ALTER THIS PATTERN WITHOUT A SECURITY REVIEW. INAPPROPRIATE CHANGES CAN ALLOW SQL
        /// INJECTION ATTACKS.
        /// </remarks>
        private const string QStringConstPatternString = @"^[A-Z]+$";

        private static readonly Regex QStringConstPattern = new Regex(QStringConstPatternString, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public StringConstParam(string stringConstant) : base(stringConstant)
        {
        }

        public override string Escape(string value)
        {
            return $"'{value}'";

        }

        public override bool IsValid(string stringConstant)
        {
            return QStringConstPattern.IsMatch(stringConstant);
        }

        public object ParameterValue => Value;

    }
}