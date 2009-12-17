using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Hosting;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Spark;
using Spark.FileSystem;
using Spark.Web.Mvc.Descriptors;
using System.Linq;

namespace Rf.Sites.Build
{
  public class CompileSparkView : Task
  {
    private readonly List<string> relevantAssemblies = new List<string>();

    [Required]
    public string OutputAssembly { get; set; }
    
    [Required]
    public string ViewsLocation { get; set; }
    
    [Required]
    public string TargetNamespace { get; set; }

    [Required]
    public string CompilationDir { get; set; }

    public ITaskItem[] UseAssemblies
    {
      get;
      set;
    }

    public override bool Execute()
    {
      var useAssemblies = UseAssemblies ?? new ITaskItem[0];

      var s = new AppDomainSetup
      {
        ApplicationBase = Path.Combine(Environment.CurrentDirectory, CompilationDir)
      };

      var args = new CompileArguments
                   {
                     CompilationDir = CompilationDir,
                     OutputAssembly = OutputAssembly,
                     ReferencedAssemblies = useAssemblies.Select(u => u.ItemSpec).ToArray(),
                     TargetNamespace = TargetNamespace,
                     ViewsLocation = Path.Combine(Environment.CurrentDirectory, ViewsLocation)
                   };

      var compile = new AppDomainExpander<SparkCompilation>();
      compile.Create(s, args);
      return true;
    }
  }

  [Serializable]
  public class CompileArguments
  {
    public string OutputAssembly { get; set; }

    public string ViewsLocation { get; set; }

    public string TargetNamespace { get; set; }

    public string CompilationDir { get; set; }

    public string[] ReferencedAssemblies { get; set; }
  }
}
