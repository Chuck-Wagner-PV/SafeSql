using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Dapper;
using SafeSql.Core.Parameters;

namespace SafeSql.Core
{
    public class SqlStatement : SqlFragment
    {
        private readonly List<SqlFragment> statement = new List<SqlFragment>();
        public SqlStatement(SqlFragment first) : base(null)
        {
            statement.Add(first);
        }

        public override bool IsValid(string fragment)
        {
            return true;
        }

        public SqlStatement Add(SqlFragment next)
        {
            statement.Add(next);
            return this;
        }

        public override string ToString()
        {
            string body = null;
            this.Emit(val => body = val);
            return $"/* {body} */";
        }

        public override void EmitParameterized(ParameterizedSqlProcessor processor, string parameterPrefix = "p")
        {
            if (string.IsNullOrWhiteSpace(parameterPrefix))
            {
                throw new ArgumentOutOfRangeException("parameterPrefix must be alphanumeric");
            }

            var sqlParameters = new DynamicParameters();

            StringBuilder sb = new StringBuilder();
            bool precedeWithSpace = false;
            int parameterCount = 0;
            foreach (var fragment in statement)
            {
                if (precedeWithSpace)
                {
                    sb.Append(' ');
                }

                var sqlParam = fragment as ISqlParameter;
                if (sqlParam != null)
                {
                    var parameterName = parameterPrefix + parameterCount.ToString();
                    sb.Append($"@{parameterName}");
                    sqlParameters.Add(parameterName, sqlParam.ParameterValue);
                    precedeWithSpace = true;
                    parameterCount++;
                }
                else
                {
                    fragment.Emit((val) =>
                    {
                        sb.Append(val);
                        precedeWithSpace = val[val.Length - 1] != ' ';
                    });
                }
            }
            processor(sb.ToString(), sqlParameters);

        }


        public override void Emit(SqlProcessor processor)
        {
            StringBuilder sb = new StringBuilder();
            bool precedeWithSpace = false;
            foreach (var fragment in statement)
            {
                if (precedeWithSpace)
                {
                    sb.Append(' ');
                }

                fragment.Emit((val) =>
                {
                    sb.Append(val);
                    precedeWithSpace = val[val.Length - 1] != ' ';
                });
            }
            processor(sb.ToString());
        }
    }
}
