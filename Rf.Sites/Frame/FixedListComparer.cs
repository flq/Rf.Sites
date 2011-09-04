using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rf.Sites.Frame
{
    public class FixedListComparer : FixedListComparer<string>
    {
        public FixedListComparer(params string[] list) : base(s => s, list) { }
    }

    public class FixedListPropertyInfoComparer : FixedListComparer<PropertyInfo>
    {
        public FixedListPropertyInfoComparer(params string[] list) : base(pi => pi.Name, list) { }
    }

    public class FixedListComparer<T> : IComparer<T>
    {
        private readonly Func<T, string> _stringSelector;
        private readonly List<string> _list;

        public FixedListComparer(Func<T,string> stringSelector, params string[] list)
        {
            _stringSelector = stringSelector;
            _list = new List<string>(list);
        }


        public int Compare(T x, T y)
        {
            var xi = _list.IndexOf(_stringSelector(x));
            var yi = _list.IndexOf(_stringSelector(y));
            return xi.CompareTo(yi);
        }
    }
}