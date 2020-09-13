using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;

namespace ET.ODataExamples.Infrastructures
{
    public static class ODataFilterHelper
    {
        public static string ContainsQueryString(string url)
        {
            Regex replaceToLowerRegex =
         new Regex(@"contains\((?<columnName>\w+),.*(?<value>(\'|%27).+(\'|%27))\)");

            var decodeUrl = HttpUtility.UrlDecode(url);
            var splitFilter = decodeUrl.Split(" or ");

            List<string> queryList = new List<string>();
            foreach (var item in splitFilter)
            {
                var replacement = @"contains(tolower(${columnName}),tolower(${value}))";
                queryList.Add(replaceToLowerRegex.Replace(item, replacement));
            }
            string newQueryString = string.Join(" or ", queryList);
            return newQueryString;
        }
    }
}
