using System;
using System.Security.Policy;

namespace Rf.Sites.Build
{
  class AppDomainExpander<T> where T : DomainLifetimeHook, new()
  {
    private AppDomain domain;

    public void Create(AppDomainSetup setup, object data)
    {
      domain = AppDomain.CreateDomain(Guid.NewGuid().ToString(), null, setup);
      domain.Load(GetType().Assembly.FullName);
      domain.SetData("data", data);
      string typename = typeof(DomainCommunicator).FullName;
      string assemblyName = typeof(DomainCommunicator).Assembly.FullName;

      var inner = (DomainCommunicator)domain.CreateInstanceAndUnwrap(assemblyName, typename);
      inner.Create();
    }

    public void End()
    {
      AppDomain.Unload(domain);
    }

    class DomainCommunicator : MarshalByRefObject
    {
      T domainHook;

      public void Create()
      {
        domainHook = new T();
        // Attaching the handler is enough to keep a reference to ourselves
        // which in turn keeps T alive...
        AppDomain.CurrentDomain.DomainUnload += onDomainUnload;
        domainHook.SetData(AppDomain.CurrentDomain.GetData("data"));
        domainHook.Start();
      }

      void onDomainUnload(object sender, EventArgs e)
      {
        domainHook.Stop();
        // ...until the Domain dies: dereference myself to be more explicit
        AppDomain.CurrentDomain.DomainUnload -= onDomainUnload;
      }
    }
  }
}