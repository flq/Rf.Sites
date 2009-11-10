using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using System.Linq;
using Rf.Sites.Tests.Frame;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class WhenThePaginatorIsUsed : DbTestBase
  {

    [SetUp]
    public void BeforeTest()
    {
      create10Contents();
    }

    [TearDown]
    public void AfterTest()
    {
      //Session.Delete("from Content");
      Paginator.SetPaginatorCountCache(new NullCache());
    }

    [Test]
    public void AllowsPaginationOfData()
    {
      var repository = new Repository<Content>(Factory);
      Paginator<Content> paginator = Paginator.For(repository).Get();

      var r = paginator.GetPage();
      Assert.That(r.GetType().Namespace, Is.Not.Matches("System.Linq"));
      var result = r.ToArray();
      Assert.That(result, Has.Length(paginator.PageSize));
      Assert.AreEqual("Title2", result[2].Title);

      paginator = Paginator.For(repository).SetPageToLoad(1).Get();
      result = paginator.GetPage().ToArray();
      Assert.That(result, Has.Length(paginator.PageSize));
      Assert.AreEqual("Title7", result[2].Title);
    }

    [Test]
    public void AllowsPaginationOfFilteredData()
    {
      var repository = new Repository<Content>(Factory);

      var query = from c in repository where c.MetaKeyWords == "Foo" select c;

      var paginator = Paginator.For(query).Get();

      var r = paginator.GetPage();
      var result = r.ToArray();
      result.ShouldHaveCount(paginator.PageSize);

      result[0].Title.ShouldBeEqualTo("Title0");
      result[1].Title.ShouldBeEqualTo("Title2");

    }

    [Test]
    public void AllowsPaginationOfAnonymousData()
    {
      var repository = new Repository<Content>(Factory);

      var query = from c in repository where c.MetaKeyWords == "Foo" select new { c.Title };

      var paginator = Paginator.For(query).Get();

      var r = paginator.GetPage();
      var result = r.ToArray();
      result.ShouldHaveCount(paginator.PageSize);

      result[0].Title.ShouldBeEqualTo("Title0");
      result[1].Title.ShouldBeEqualTo("Title2");

    }

    [Test]
    public void PaginatorTellsNumberOfPages()
    {
      var repository = new Repository<Content>(Factory);

      int count = repository.Count;

      var paginator = Paginator.For(repository)
        .SetPageSize(2)
        .Get();

      paginator.PageCount.ShouldBeEqualTo(count / 2);
    }

    [Test]
    public void PaginatorCachesRowCounts()
    {
      var cacheMock = new Mock<ICache>();
      cacheMock.Setup(c => c.Get<int>(It.IsAny<string>())).Returns(15);
      cacheMock.Setup(c => c.HasValue("A")).Returns(true);
      Paginator.SetPaginatorCountCache(cacheMock.Object);

      var paginator = Paginator.For(new Repository<Content>(Factory))
        .SetCacheKey("A")
        .SetPageSize(3)
        .Get();
      paginator.PageCount.ShouldBeEqualTo(5);
      cacheMock.VerifyAll();
    }

    [Test]
    public void PaginatorMustRoundUpPageCount()
    {
      var cacheMock = new Mock<ICache>();
      cacheMock.Setup(c => c.Get<int>(It.IsAny<string>())).Returns(16);
      cacheMock.Setup(c => c.HasValue("A")).Returns(true);
      Paginator.SetPaginatorCountCache(cacheMock.Object);
      var paginator = Paginator.For(new Repository<Content>(Factory))
        .SetPageSize(3)
        .SetCacheKey("A")
        .Get();
      paginator.PageCount.ShouldBeEqualTo(6);
      cacheMock.VerifyAll();
    }


    private void create10Contents()
    {
      using (var t = Session.BeginTransaction())
      {
        for (int i = 0; i < 10; i++)
        {
          var content = Maker.CreateContent();
          content.MetaKeyWords = i%2 == 0 ? "Foo" : "Bar";
          content.Title = "Title" + i;
          Session.Save(content);
        }
        t.Commit();
      }
      Session.Clear();
    }
  }
}