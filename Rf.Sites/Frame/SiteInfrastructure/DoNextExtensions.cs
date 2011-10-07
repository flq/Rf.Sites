using System;
using FubuMVC.Core;

namespace Rf.Sites.Frame.SiteInfrastructure
{
    public static class DoNextExtensions
    {
        public static DoNext When(this DoNext value, Func<bool> activity)
        {
            if (value == DoNext.Stop)
                return value;
            return activity() ? DoNext.Continue : DoNext.Stop;
        }

        public static DoNext Or(this DoNext value, Func<DoNext> activity)
        {
            return value == DoNext.Stop ? value : activity();
        }

        public static void Finally(this DoNext value, Action activity)
        {
            if (value != DoNext.Stop)
                activity();
        }
    }

}