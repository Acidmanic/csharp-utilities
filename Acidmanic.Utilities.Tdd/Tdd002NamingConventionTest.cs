using System;
using Acidmanic.Utilities.NamingConventions;

namespace Acidmanic.Utilities.Tdd
{
    public class Tdd002NamingConventionTest : TddBase
    {
     


        public override void Main()
        {
            
            var names = new[]
            {
                "ahmad-mahmud-kababi", "FAT_SNAKE", "_internalShit",
                "lame_snake", "MrPascal", "stupidCamel"
            };

            var namingConvention = new NamingConvention();

            
            
            foreach (var name in names)
            {
                var parsed = namingConvention.Parse(name);

                Console.WriteLine("--------------------------");

                if (parsed)
                {
                    Console.WriteLine("Parsed " + name + ", Detected: " + parsed.Value.Convention.Name);

                    Console.WriteLine("-----------");

                    foreach (var convention in ConventionDescriptor.Standard.StandardConventions)
                    {
                        Console.WriteLine(convention.Name + ": " +
                                          namingConvention.Render(parsed.Value.Segments, convention));
                    }
                }
            }
        }
    }
}