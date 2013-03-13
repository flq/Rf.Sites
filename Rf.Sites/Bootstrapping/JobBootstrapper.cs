using System;
using System.Collections.Generic;
using Bottles;
using Bottles.Diagnostics;
using WebBackgrounder;

namespace Rf.Sites.Bootstrapping
{
    
    public class JobsBootstrapper : IActivator, IDisposable
    {
        private readonly JobManager _jobManager;

        public JobsBootstrapper(IJob[] jobs)
        {
            var coordinator = new SingleServerJobCoordinator();
            _jobManager = new JobManager(jobs, coordinator);
        }

        void IActivator.Activate(IEnumerable<IPackageInfo> packages, IPackageLog log)
        {
            _jobManager.Start();
        }

        public void Dispose()
        {
            _jobManager.Stop();
            if (_jobManager != null)
                _jobManager.Dispose();
            
        }
    }
}