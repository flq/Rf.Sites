namespace Rf.Sites.Actions.Args
{
  public static class ArgsFrom
  {
    public static NullArgs Null
    {
      get { return new NullArgs(); }
    }

    public static IDActionArgs Id(int id)
    {
      return new IDActionArgs { Id = id };
    }

    public static ValueArgs Value(string value)
    {
      return new ValueArgs { Value = value };
    }

    public static ValueArgs Value(string value, int page)
    {
      return new ValueArgs { Value = value, Page = page };
    }

    public static PageArgs Page(int page)
    {
      return new PageArgs { Page = page };
    }

    public static YearArgs Year(int year, int page)
    {
      return new YearArgs {Page = page, Year = year};
    }

    public static MonthArgs Month(int year, int month, int page)
    {
      return new MonthArgs { Year = year, Month = month, Page = page};
    }

  }
}