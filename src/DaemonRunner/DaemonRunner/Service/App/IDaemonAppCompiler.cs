using System;
using System.Collections.Generic;
using System.Reflection;

namespace NetDaemon.Service.App
{
    public interface IDaemonAppCompiler
    {
        /// <summary>
        /// Temporary
        /// </summary>
        IEnumerable<Type> GetApps(IEnumerable<Assembly> assemblies);
    }
}