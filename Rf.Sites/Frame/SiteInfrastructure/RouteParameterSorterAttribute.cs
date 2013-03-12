using System;
using System.Collections.Generic;

namespace Rf.Sites.Frame.SiteInfrastructure
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class RouteParameterSorterAttribute : Attribute
    {
        private readonly FixedListComparer<string> _sorter;

        public RouteParameterSorterAttribute(params string[] ordering)
        {
            _sorter = new FixedListComparer<string>(s => s, ordering);
        }
        
        public IComparer<string> GetComparer()
        {
            return _sorter;
        }
    }
}