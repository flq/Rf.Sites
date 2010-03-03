using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using Rf.Sites.Domain;
using Rf.Sites.Frame;
using Rf.Sites.Models;
using Rf.Sites.Tests.Frame;
using Spark;
using Spark.Compiler;
using Spark.FileSystem;
using Spark.Web.Mvc;
using StructureMap;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class BasicCheck
  {
    [Test]
    public void Do()
    {
      var b = new DbTestBase();
      using (var s = b.Session)
      Debug.Write("OK");
    }

    [Test]
    public void StructureMapBehaviourCheck()
    {
      var cnt = new Container();
      cnt.Configure(ce=>ce.Scan(s=>
                                  {
                                    s.AssemblyContainingType(typeof(IVmExtender<>));
                                    s.AddAllTypesOf(typeof (IVmExtender<>));
                                    s.ConnectImplementationsToTypesClosing(typeof (IObjectConverter<,>));
                                  }));
      var l = cnt.GetAllInstances<IVmExtender<ContentViewModel>>();
      l.Count.ShouldBeGreaterThan(0);
      var l2 = cnt.GetAllInstances<IVmExtender<CommentVM>>();
      l2.Count.ShouldBeGreaterThan(0);
      var c = cnt.GetInstance<ExtenderConsumer>();
      c.Extender.Count.ShouldBeGreaterThan(0);
      var conv = cnt.GetInstance<IObjectConverter<Comment, CommentVM>>();
      conv.ShouldNotBeNull();
    }

    public class ExtenderConsumer
    {
      public IList<IVmExtender<ContentViewModel>> Extender { get; set; }

      public ExtenderConsumer(IList<IVmExtender<ContentViewModel>> extender)
      {
        Extender = extender;
      }
    }

    [Test]
    public void AllViewsCurrentlyCompile()
    {
      //http://sparkviewengine.com/documentation/precompiling

      const string basePath = @"..\..\..\Rf.Sites\Views";
      var viewFactory = new SparkViewFactory
                          {
                            ViewFolder = new FileSystemViewFolder(basePath)
                          };
      
      var batch = new SparkBatchDescriptor()
        .For<Folders.Content>()
        .For<Folders.Tagcloud>()
        .For<Folders.Recent>()
        .For<Folders.Shared>().Exclude("Application")
        .FromAssembly(typeof (ActionDispatcher).Assembly);
            
      try
      {
        var viewsAssembly = viewFactory.Precompile(batch);
        viewsAssembly.ShouldNotBeNull();
      }
      catch (CompilerException x)
      {
        Console.WriteLine("Compile exception in file {0}, line {1}", x.Filename, x.Line);
        Assert.Fail("Compilation error:" + x.Message);
      }
    }

    // HACK: Monster Hack. Spark resolves view directories based on Controller types.
    // Since I don't have any controllers, these are the types that at the end map
    // to folders inside the "Views" folder
    static class Folders
    {
      public abstract class Content { }
      public abstract class Tagcloud { }
      public abstract class Shared { }
      public abstract class Recent { }
    }
  }
}