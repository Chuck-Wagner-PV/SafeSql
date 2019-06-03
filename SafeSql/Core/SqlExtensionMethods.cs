using System;
using System.Collections.Generic;
using System.Text;
using SafeSql.Core.Parameters;

namespace SafeSql.Core
{
    public static class SqlExtensionMethods
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Extension method to convert a string into a String Const Parameter. </summary>
        ///
        /// <param name="value">  The value to act on. </param>
        ///
        /// <returns>   A SqlFragment. </returns>
        public static SqlFragment SqlStringConst(this String value)
        {
            return new StringConstParam(value);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Extension method to convert an int into a Int Parameter </summary>
        ///
        /// <param name="value">    The value to act on. </param>
        ///
        /// <returns>   A SqlFragment. </returns>
        public static SqlFragment SqlInt(this int value)
        {
            return new IntParam(value);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Extension method to convert an int into a String Int Parameter 
        ///             (ex. 12 -> '12')</summary>
        ///
        /// <param name="value">    The value to act on. </param>
        ///
        /// <returns>   A SqlFragment. </returns>
        public static SqlFragment SqlStringInt(this int value)
        {
            return new StringIntParam(value);
        }
    }
}
