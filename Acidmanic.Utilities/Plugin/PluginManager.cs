using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Acidmanic.Utilities.Results;

namespace Acidmanic.Utilities.Plugin
{
    public class PluginManager
    {

        private static PluginManager? _instance = null;
        private static readonly object Locker = new object();

        public static PluginManager Instance
        {
            get
            {
                lock (Locker)
                {
                    if (_instance == null)
                    {
                        _instance = new PluginManager();
                    }

                    return _instance;
                }
            }
        }


        private readonly string _pluginsDirectory;

        private readonly Dictionary<string, Assembly> _assembliesByPluginName = new Dictionary<string, Assembly>();
        private readonly Dictionary<string, PluginLoaderAssemblyContext> _loaderContextsByPluginName = new Dictionary<string, PluginLoaderAssemblyContext>();
        
        public IReadOnlyDictionary<string, Assembly> PluginAssemblies => _assembliesByPluginName;
        

        public PluginManager()
        {
            
            _pluginsDirectory = CheckPluginDirectory();
            
            ScanPlugins();
        }

        private string CheckPluginDirectory()
        {
            var assemblyLocation = Assembly.GetEntryAssembly()?.Location;

            if (!string.IsNullOrEmpty(assemblyLocation))
            {
                assemblyLocation = new DirectoryInfo(assemblyLocation)?.Parent?.FullName;
            }

            var assemblyDirectory = assemblyLocation ?? ".";

            assemblyDirectory = new DirectoryInfo(assemblyDirectory).FullName;

            var pluginsDirectory = Path.Combine(assemblyDirectory, "Plugins");

            if (!Directory.Exists(pluginsDirectory))
            {
                Directory.CreateDirectory(pluginsDirectory);
            }

            return pluginsDirectory;
        }


        public void ScanPlugins()
        {
            _assembliesByPluginName.Clear();
            _loaderContextsByPluginName.Clear();

            var directories = Directory.EnumerateDirectories(_pluginsDirectory);

            foreach (var directory in directories)
            {
                var pluginDirectory = new DirectoryInfo(directory);

                var pluginNameLower = pluginDirectory.Name.ToLower();
                
                var files = pluginDirectory.EnumerateFiles();

                foreach (var file in files)
                {
                    if (file.Name.ToLower() == pluginNameLower)
                    {
                        var assemblyLoaded = TryLoadingAssembly(file.FullName);

                        if (assemblyLoaded)
                        {
                            _assembliesByPluginName.Add(pluginNameLower,assemblyLoaded.Primary);
                            _loaderContextsByPluginName.Add(pluginNameLower,assemblyLoaded.Secondary);
                        }
                    }
                }
            }
            
        }

        private Result<Assembly,PluginLoaderAssemblyContext> TryLoadingAssembly(string file)
        {
            try
            {
                
                var context = new PluginLoaderAssemblyContext(file);

                var assemblyName = new AssemblyName(Path.GetFileNameWithoutExtension(file));

                var assembly = context.LoadFromAssemblyName(assemblyName);

                if (assembly != null)
                {

                    return new Result<Assembly, PluginLoaderAssemblyContext>
                        (true, context, assembly);
                }
            }
            catch (Exception _) { /**/ }

            return new Result<Assembly,PluginLoaderAssemblyContext>().FailAndDefaultBothValues();
        }
    }
}