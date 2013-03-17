using System;
using System.Collections.Generic;
using System.Text;
using FubuMVC.Core.Registration.Routes;
using System.Linq;

namespace Rf.Sites.Frame.SiteInfrastructure
{
    public static class RouteDefinitionExtensions
    {
        public static void Replace(this IRouteDefinition def, string tobeReplaced, string replacement)
        {
            var q = new Queue<string>(def.Pattern.Split('/'));

            for (var i = 0; i < q.Count; i++)
                def.RemoveLastPatternPart();

            while (q.Count > 0)
                def.Append(q.Dequeue().ReplaceString(tobeReplaced, replacement, StringComparison.InvariantCultureIgnoreCase));
        }

        public static void ReorderParts(this IRouteDefinition def, IComparer<string> comparer)
        {
            var parts = new List<string>(def.Pattern.Split('/').Where(s => s.StartsWith("{")) .Select(p => p.Replace("{", "").Replace("}", "")));

            for (var i = 0; i < parts.Count; i++)
                def.RemoveLastPatternPart();

            parts.Sort(comparer);

            foreach (var p in parts)
                def.Append("{" + p + "}");
        }

        /// <summary>
        /// Taken from http://stackoverflow.com/questions/244531/is-there-an-alternative-to-string-replace-that-is-case-insensitive
        /// to do a case insensitive replacement
        /// </summary>
        public static string ReplaceString(this string str, string oldValue, string newValue, StringComparison comparison)
        {
            var sb = new StringBuilder();

            int previousIndex = 0;
            int index = str.IndexOf(oldValue, comparison);
            while (index != -1)
            {
                sb.Append(str.Substring(previousIndex, index - previousIndex));
                sb.Append(newValue);
                index += oldValue.Length;

                previousIndex = index;
                index = str.IndexOf(oldValue, index, comparison);
            }
            sb.Append(str.Substring(previousIndex));

            return sb.ToString();
        }
    }


}