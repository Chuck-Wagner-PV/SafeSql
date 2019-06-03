using System;

namespace SafeSql.Core
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    /// Exception for indicating that the code caught what appears to be a SQL injection.
    /// </summary>
    public class SqlInjectionException : Exception
    {
    }
}