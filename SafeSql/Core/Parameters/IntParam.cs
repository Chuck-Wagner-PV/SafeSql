namespace SafeSql.Core.Parameters
{
    /// <summary>   An integer parameter (will appear without quotes), eg. 123 </summary>
    public class IntParam : SqlFragment, ISqlParameter
    {
        public IntParam(int value) : base(value.ToString())
        {
            IntValue = value;
        }

        public int IntValue { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> We're always safe, we were constructed with an int.</summary>
        ///
        /// <param name="value"> The int as string. </param>
        ///
        /// <returns>   True. </returns>
        public override bool IsValid(string value)
        {
            return true;
        }

        public object ParameterValue => IntValue;
    }
}