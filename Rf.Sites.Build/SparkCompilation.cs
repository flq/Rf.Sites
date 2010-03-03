using System.Collections.Generic;
using System.IO;
using Spark;
using Spark.FileSystem;
using Spark.Web.Mvc;

namespace Rf.Sites.Build
{
  class SparkCompilation : DomainLifetimeHook
  {
    internal override void Start()
    {
      var args = (CompileArguments) data;

      var viewFactory = new SparkViewFactory
                          {
                            ViewFolder = new FileSystemViewFolder(args.ViewsLocation)
                          };

      var batch = new SparkBatchDescriptor()
        .For<Folders.Content>()
        .For<Folders.Tagcloud>()
        .For<Folders.Shared>().Exclude("Application")
        .FromAssemblyNamed("Rf.Sites");
      batch.OutputAssembly = args.OutputAssembly;

      var viewsAssembly = viewFactory.Precompile(batch);
      var path = viewsAssembly.CodeBase.Replace("file:///", "");

      File.Copy(path, Path.Combine(args.CompilationDir, args.OutputAssembly), true);
    }

    internal override void Stop()
    {
      
    }
  }

  static class Folders
  {
    public abstract class Content { }
    public abstract class Recent { }
    public abstract class Tagcloud { }
    public abstract class Shared { }
  }
}