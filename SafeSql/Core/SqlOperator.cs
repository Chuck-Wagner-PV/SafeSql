namespace SafeSql.Core
{
    /// <summary>   A set of static SQL operators.</summary>
    public class SqlOperator : SqlFragment
    {
        public static readonly SqlOperator Equal = new SqlOperator("=");
        public static readonly SqlOperator NotEqual = new SqlOperator("<>");
        public static readonly SqlOperator Is = new SqlOperator("IS");
        public static readonly SqlOperator IsNot = new SqlOperator("IS NOT");

        private SqlOperator(string op) : base(op)
        {
        }

        public override bool IsValid(string fragment)
        {
            return true;
        }
    }
}