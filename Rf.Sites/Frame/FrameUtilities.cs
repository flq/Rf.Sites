using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Rf.Sites.Actions.Args;
using System.Linq;
using Rf.Sites.Domain.Frame;

namespace Rf.Sites.Frame
{
  public static class FrameUtilities
  {
    public static string ActionLink<T>(this HtmlHelper helper, string linkText) where T : IAction
    {
      return helper.ActionLink<T>(linkText, ArgsFrom.Null);
    }

    public static string ActionLink<T>(this UrlHelper helper) where T : IAction
    {
      var str = typeof (T).Name.Replace("Action", "").PasCalCaseTokenization();
      return helper.Action(str[1], str[0]);
    }

    public static string ActionLink<T>(this HtmlHelper helper, string linkText, IArgToRoute args) where T : IAction
    {
      return helper.ActionLink<T>(linkText, args, null);
    }

    public static string ActionLink<T>(this HtmlHelper helper, string linkText, IArgToRoute args, IDictionary<string,object> linkAttributes) where T : IAction
    {
      var tokens = typeof(T).Name.PasCalCaseTokenization();
      return helper.ActionLink(linkText, tokens[1], tokens[0], args.ToDictionary(), linkAttributes);
    }

    public static string RelativeUrlToAction<T>(params string[] additionalRouteValues) where T : IAction
    {
      var str = typeof (T).Name.Replace("Action", "").PasCalCaseTokenization();
      var allValues = str.Concat(additionalRouteValues);
      return "/" + string.Join("/", allValues.ToArray());
    }

    public static string[] PasCalCaseTokenization(this string name)
    {
      var chars = name.ToCharArray();

      List<string> results = new List<string>();
      StringBuilder b = new StringBuilder();
      b.Append(chars[0]);

      for (int i = 1; i < chars.Length; i++)
      {
        if (char.IsUpper(chars[i]))
        {
          results.Add(b.ToString());
          b.Length = 0;
        }
        b.Append(chars[i]);
      }
      results.Add(b.ToString());

      return results.ToArray();
    }

    public static string GetValue1(this ControllerContext ctx)
    {
      return getValue(ctx, "val1");
    }

    public static string GetValue2(this ControllerContext ctx)
    {
      return getValue(ctx, "val2");
    }

    public static string GetValue3(this ControllerContext ctx)
    {
      return getValue(ctx, "val3");
    }

    private static string getValue(ControllerContext ctx, string key)
    {
      var value = ctx.RouteData.Values[key];
      return value != null ? value.ToString() : "";
    }

    /// <summary>
    /// Supports a conversion to types, using the proper conversion method from strings. If
    /// conversion is impossible or fails, the provided default value is returned.
    /// Supported types:
    /// - int
    /// </summary>
    public static T SafeCast<T>(this string value, T defaultvalue)
    {
      if (string.IsNullOrEmpty(value))
        return defaultvalue;
      if (defaultvalue is int)
      {
        try
        {
          var retVal = int.Parse(value);
          return (T)(object)retVal; //OMG! I hope the JIT removes all this shit
        }
        catch (FormatException)
        {
          return defaultvalue;
        }
      }
      return defaultvalue;
    }

    public static void Apply<T>(this IEnumerable<IVmExtender<T>> extenders, T viewModel)
    {
      if (extenders == null)
        return;

      var orderedExtenders =
        from e in extenders
        let orderkey =
          e.GetType().HasAttribute<OrderAttribute>() ? e.GetType().GetAttribute<OrderAttribute>().OrderKey : 1
        orderby orderkey
        select e;

      foreach (var e in orderedExtenders)
        e.Inspect(viewModel);
    }
  }


}