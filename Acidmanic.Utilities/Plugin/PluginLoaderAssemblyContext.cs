using System;
using System.Reflection;
using System.Runtime.Loader;

namespace Acidmanic.Utilities.Plugin
{
    public class PluginLoaderAssemblyContext:AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver _assemblyDependencyResolver;

        public PluginLoaderAssemblyContext(string pluginAssemblyFilePath)
        {
            _assemblyDependencyResolver = new AssemblyDependencyResolver(pluginAssemblyFilePath);
        }


        protected override Assembly Load(AssemblyName assemblyName)
        {
            var path = _assemblyDependencyResolver.ResolveAssemblyToPath(assemblyName);

            if (!string.IsNullOrEmpty(path))
            {
                return LoadFromAssemblyPath(path);
            }

            return null;
        }


        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            var path = _assemblyDependencyResolver.ResolveUnmanagedDllToPath(unmanagedDllName);

            if (!string.IsNullOrEmpty(path))
            {
                return LoadUnmanagedDllFromPath(path);
            }

            return IntPtr.Zero;
        }
    }
    
}