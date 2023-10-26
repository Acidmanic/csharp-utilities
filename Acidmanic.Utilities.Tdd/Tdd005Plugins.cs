using System;
using System.Runtime.InteropServices;
using Acidmanic.Utilities.Factories;
using Acidmanic.Utilities.Plugin;
using Sample.Contract;

namespace Acidmanic.Utilities.Tdd
{
    public static class ServicePrintingExtensions
    {

        public static void PrintMessage(this IService service)
        {
            var model = service.ProvideVeryImportantData();

            var line = $"Very Important Message: {model.Very} {model.Important} {model.Data}";

            Console.WriteLine(line);
        }
    }


    public class Tdd005Plugins : TddBase
    {
        private class ServiceFactory : FactoryBase<IService, string>
        {
            private class NullService : IService
            {
                public string Name => "";

                public Model ProvideVeryImportantData()
                {
                    return new Model
                    {
                        Very = "NULL",
                        Data = "NULL",
                        Important = "NULL"
                    };
                }
            }

            public ServiceFactory() : base(FactoryMatching.MatchByInstance)
            {
                var plugins = PluginManager.Instance.PluginAssemblies.Values;

                foreach (var plugin in plugins)
                {
                    ScanAssembly(plugin);
                }
            }

            protected override bool MatchesByType(Type productType, string value)
            {
                return false;
            }

            protected override bool MatchesByInstance(IService product, string value)
            {
                return product != null && product.Name.ToLower() == value.ToLower();
            }

            protected override IService DefaultValue()
            {
                return new NullService();
            }
        }

        public override void Main()
        {

            var factory = new ServiceFactory();

            var service = factory.Make("usecase-1");

            service.PrintMessage();
        }
    }
}