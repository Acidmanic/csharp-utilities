using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Acidmanic.Utilities.Filtering.Utilities;
using Acidmanic.Utilities.Reflection.Attributes;

namespace Acidmanic.Utilities.Tdd;

public class Tdd007FilterResultsType:TddBase
{

    [AlteredType(typeof(string))]
    private struct Id
    {
        public Id(long value)
        {
            Value = value;
        }

        public long Value { get; }


        public static implicit operator long(Id value) => value.Value;
        public static implicit operator Id(long value) => new Id(value);
        
    }


    private class Model
    {
        [UniqueMember]
        [TreatAsLeaf]
        public Id Id { get; set; }
        
        public string Name { get; set; }
    }
    
    
    public override void Main()
    {
        var filterResultType = FilteringTypeUtilities.GetFilterResultsType(typeof(Model));

        var properties = filterResultType.GetProperties();

        foreach (var property in properties)
        {
            var force = property.GetCustomAttributes<TreatAsLeafAttribute>().Any();

            var flag = force ? "[>>TAL<<]" : "[       ]";
            
            Console.WriteLine(flag + " " + property.PropertyType.Name + ": " + property.Name);
            
        }
    }
}