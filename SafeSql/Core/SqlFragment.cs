using System;
using Dapper;
using SafeSql.Core.Parameters;

namespace SafeSql.Core
{
    /// <summary> The base class for all the SafeSql objects - a fragment of a SQL statement.</summary>
    public abstract class SqlFragment
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary> The raw value of the fragment as a string.</summary>
        ///
        /// <value> The value. </value>
        protected string Value { get; }

        public SqlFragment(string value)
        {
            Value = value;

            // This should probably be replaced with a smart handler. Possibly one that allows us to
            // capture a call stack, and figure out where the line was coming from that gave us bad data
            // rather than just throwing an exception.
            // This is good for the purposes of demonstration.
            if (!IsValid(value)) throw new SqlInjectionException();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Allows subclasses to tell us if this value looks valid. </summary>
        ///
        /// <param name="value"> The value of this fragment (not encoded). </param>
        ///
        /// <returns>   True if valid, false if not. </returns>
        public abstract bool IsValid(string value);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Allows examination BUT NOT USE OF any SQL fragment. </summary>
        ///
        /// <remarks>   If you want to get the value out of this without comments use Emit. </remarks>
        ///
        /// <returns>   A SQL comment containing the current SQL fragment. </returns>
        public override string ToString()
        {
            return $"/* {Value} */";
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Allows subclasses to escape the value properly if necessary.</summary>
        ///
        /// <param name="value">    The value. </param>
        ///
        /// <returns>   A string. </returns>
        public virtual string Escape(string value)
        {
            return value;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Emits the given fragment to a function to handle it. </summary>
        ///
        /// <param name="processor">  Function to handle the SQL. </param>
        public virtual void Emit(SqlProcessor processor)
        {
            processor(Escape(Value));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Emits the given fragment to a function to handle it
        ///             as a parameterized SQL statement. </summary>
        ///
        /// <param name="processor">        Function to handle the SQL and parameters. </param>
        /// <param name="parameterPrefix">  (Optional) The parameter prefix. </param>
        public virtual void EmitParameterized(ParameterizedSqlProcessor processor, string parameterPrefix = "p")
        {
            processor(Escape(Value), new DynamicParameters());
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Concatenation operator. </summary>
        ///
        /// <param name="p1">   The first SQL fragment. </param>
        /// <param name="p2">   The next SQL fragment. </param>
        ///
        /// <returns>   The resulting SQL statement. </returns>
        public static SqlFragment operator + (SqlFragment p1, SqlFragment p2)
        {
            return (p1 as SqlStatement ?? new SqlStatement(p1)).Add(p2);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Concatenation operator for a string value parameter. </summary>
        ///
        /// <param name="p1">   The first SQL fragment. </param>
        /// <param name="p2">   The string value. </param>
        ///
        /// <returns>   The resulting SQL statement. </returns>
        public static SqlFragment operator +(SqlFragment p1, string p2)
        {
            return (p1 as SqlStatement ?? new SqlStatement(p1)).Add(new StringValueParam(p2));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Concatenation operator for an integer parameter. </summary>
        ///
        /// <param name="p1">   The first SQL fragment. </param>
        /// <param name="p2">   The integer value. </param>
        ///
        /// <returns>   The resulting SQL statement. </returns>
        public static SqlFragment operator +(SqlFragment p1, int p2)
        {
            return (p1 as SqlStatement ?? new SqlStatement(p1)).Add(new IntParam(p2));
        }
    }
}