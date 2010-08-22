using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Rf.Sites.Actions.Args;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;
using Rf.Sites.Models;
using Rf.Sites.Queries;

namespace Rf.Sites.Actions
{
  public class ChronicsIndexAction : AbstractAction
  {
    private readonly ChronicsPostArgs args;
    private readonly IRepository<Content> repository;
    private readonly IVmExtender<ChronicsNode>[] extenders;
    private readonly ICache cache;

    public ChronicsIndexAction(ChronicsPostArgs args, ICache cache, IRepository<Content> repository, IVmExtender<ChronicsNode>[] extenders)
    {
      this.args = args;
      this.repository = repository;
      this.extenders = extenders;
      this.cache = cache;
    }

    public override ActionResult Execute()
    {
      var tree = cache.HasValue("chronics") ? cache.Get<ChronicsTree>("chronics") : createTree();
      var nodes = tree.Query(args.RawRequestId);
      if (nodes != null)
        Array.ForEach(nodes, extenders.Apply);
      return new JsonResult {Data = nodes};
    }

    private ChronicsTree createTree()
    {
      var t = new ChronicsTree(new ValidDateTimesQuery().Query(repository));
      cache.Add("chronics", t);
      return t;
    }
  }
}