namespace SafeSql.Core.Parameters
{
    /// <summary>   Used for integer parameters that need to be quoted, eg. '12923'. </summary>
    public class StringIntParam : IntParam, ISqlParameter
    {
        public StringIntParam(int value) : base(value)
        {
        }

        public override string Escape(string value)
        {
            return $"'{value}'";
        }
        public object ParameterValue => Value;
    }
}