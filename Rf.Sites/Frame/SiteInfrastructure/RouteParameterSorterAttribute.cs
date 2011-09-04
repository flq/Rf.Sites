using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rf.Sites.Frame.SiteInfrastructure
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class RouteParameterSorterAttribute : Attribute
    {
        private readonly FixedListPropertyInfoComparer _sorter;

        public RouteParameterSorterAttribute(params string[] ordering)
        {
            _sorter = new FixedListPropertyInfoComparer(ordering);
        }
        
        public IComparer<PropertyInfo> GetComparer()
        {
            return _sorter;
        }
    }
}