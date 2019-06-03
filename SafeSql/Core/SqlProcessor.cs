using Dapper;

namespace SafeSql.Core
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Function to handle SQL emitted from a SQL Fragment. </summary>
    ///
    /// <param name="sql">  The SQL. </param>
    public delegate void SqlProcessor(string sql);

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Function to handle the SQL as a parameterized statement.</summary>
    ///
    /// <param name="sql">          The SQL statement resulting. </param>
    /// <param name="parameters">   An ExpandoObject with values for all the parameters </param>
    public delegate void ParameterizedSqlProcessor(string sql, DynamicParameters parameters);
}