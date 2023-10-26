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
        private static object _locker = new object();

        public static PluginManager Instance
        {
            get
            {
                lock (_locker)
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

        public List<Assembly> Assemblies { get; } = new List<Assembly>();

        public PluginManager()
        {
            
            _pluginsDirectory = CheckPluginDirectory();
            
            ScanPlugins();
        }

        private string CheckPluginDirectory()
        {
            var assemblyLocation = Assembly.GetEntryAssembly()?.Location;

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
            var files = Directory.GetFiles(_pluginsDirectory);

            foreach (var file in files)
            {
                var assemblyLoaded = TryLoadingAssembly(file);

                if (assemblyLoaded)
                {
                    Assemblies.Add(assemblyLoaded);
                }
            }
        }

        private Result<Assembly> TryLoadingAssembly(string file)
        {
            try
            {
                var loaded = Assembly.LoadFile(file);

                if (loaded != null)
                {
                    return new Result<Assembly>(true, loaded);
                }
            }
            catch (Exception _) { /**/ }

            return new Result<Assembly>().FailAndDefaultValue();
        }
    }
}