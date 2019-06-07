using System;
using System.Text.RegularExpressions;

namespace SafeSql.Core
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    /// A SQL phrase, begins with a SQL keyword. May not include string parameters or comments.
    /// </summary>
    public class SqlPhrase : SqlFragment
    {
        ///=================================================================================================
        /// <summary>
        /// DO NOT ALTER THIS REGEX WITHOUT A SECURITY REVIEW.
        /// INAPPROPRIATE CHANGES CAN ALLOW SQL INJECTION ATTACKS.
        /// 
        /// Requires that the phrase begin with a common SQL keyword followed by a series of words, 
        /// numbers and operators
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        private const string SqlPhrasePatternString =
            @"^\s*(AND|BULK INSERT|DELETE|FROM|GROUP|HAVING|INSERT|INNER|JOIN|MERGE|ORDER|OPTION|OR|OUTER|OUTPUT|READTEXT|SELECT|TOP|UPDATE|UPDATETEXT|VALUES|WHEN|WHERE|WITH|WRITETEXT)\s+([A-Z0-9\*/\+\-=<>\(\),\s]|\[[A-Z][A-Z0-9]+\])*$";

        ///=================================================================================================
        /// <summary>
        /// DO NOT ALTER THIS REGEX WITHOUT A SECURITY REVIEW.
        /// INAPPROPRIATE CHANGES CAN ALLOW SQL INJECTION ATTACKS.
        /// 
        /// No quoted strings or commenting allowed.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        private const string InvalidPatternString =
            @"^.*(\-\-|/\*|').*$";

        /// <summary> Regex for what we expect. </summary>
        private static readonly Regex SqlPhrasePattern = new Regex(SqlPhrasePatternString, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        /// <summary> Regex for what we we disallow. </summary>
        private static readonly Regex InvalidPattern = new Regex(InvalidPatternString, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="value"> The hardcoded SQL phrase. </param>
        public SqlPhrase(string value) : base(value)
        {
            if (string.IsInterned(value) == null)
            {
                throw new ArgumentException("SQL phrases must be compile time constants");
            }

        }

        public override bool IsValid(string sqlPhrase)
        {
            return !InvalidPattern.IsMatch(sqlPhrase) && SqlPhrasePattern.IsMatch(sqlPhrase);
        }
    }
}