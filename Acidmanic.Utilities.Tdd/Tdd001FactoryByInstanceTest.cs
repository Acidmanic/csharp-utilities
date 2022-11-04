using System;
using System.IO;
using Acidmanic.Utilities.Factories;

namespace Acidmanic.Utilities.Tdd
{
    public class Tdd001FactoryByInstanceTest:TddBase
    {

        private abstract class ProductBase
        {
            public string Name => Brand + " - " + GetType().Name;

            protected abstract string Brand { get; }
            
            public bool Supports(string brand)
            {
                return this.Brand.ToLower() == brand.ToLower();
            }
        }

        private class Mug : ProductBase
        {
            protected override string Brand => "Friends";
        }
        
        private class Speaker : ProductBase
        {
            protected override string Brand => "G-Shark";
        }
        
        private class ToothPaste : ProductBase
        {
            protected override string Brand => "Pooneh";
        }
        
        private class Camera : ProductBase
        {
            protected override string Brand => "CanOne";
        }


        private class ProductFactory : FactoryBase<ProductBase, string>
        {

            public ProductFactory():base(FactoryMatching.MatchByInstance)
            {
                
            }
            
            protected override bool MatchesByType(Type productType, string value)
            {
                return false;
            }

            protected override bool MatchesByInstance(ProductBase product, string value)
            {
                return product.Supports(value);
            }
        }

        public override void Main()
        {


            string[] brandNames = { "friends","g-shark","canone"};

            var factory = new ProductFactory();

            foreach (var brandName in brandNames)
            {
                var product = factory.Make(brandName);

                if (product == null)
                {
                    Console.WriteLine($"FACTORY RETURNED NULL For {brandName}");
                }
                else
                {
                    Console.WriteLine($"Created {product.Name}");
                }
            }
        }
    }
}