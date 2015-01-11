using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class StingHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strArr"></param>
        /// <returns></returns>
        public static List<int> ToIntList(this IEnumerable<string> strArr)
        {
            var intList = new List<int>();
            foreach (var s in strArr)
            {
                intList.Add(Convert.ToInt32(s));
            }
            return intList;
        }

        public static new string ToValue(this bool boolValue, string trueString = "是", string falseString = "否")
        {

            return boolValue ? trueString : falseString;

        }
    }
}
