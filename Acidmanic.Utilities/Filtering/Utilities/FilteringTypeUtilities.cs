using System;
using System.Linq;
using Acidmanic.Utilities.Filtering.Models;
using Acidmanic.Utilities.Reflection;
using Acidmanic.Utilities.Reflection.Attributes;

namespace Acidmanic.Utilities.Filtering.Utilities
{
    public static class FilteringTypeUtilities
    {
        public static Type GetFilterResultsType(Type entityType)
        {
            return MakeSpecifiedTypeForGenericIdType(
                typeof(FilterResult<>),
                typeof(FilterResultForceLeafId<>),
                entityType);
        }

        public static Type GetSearchIndexType(Type entityType)
        {
            return MakeSpecifiedTypeForGenericIdType(
                typeof(SearchIndex<>),
                typeof(SearchIndexForceLeafId<>),
                entityType);
        }

        private static Type MakeSpecifiedTypeForGenericIdType(
            Type idGenericType,
            Type idGenericForceLeafType,
            Type entityType)
        {
            var idLeaf = TypeIdentity.FindIdentityLeaf(entityType);

            if (idLeaf == null)
            {
                throw new Exception($"WARNING: the entity of type {entityType.FullName}" +
                                    $", does not have an identifier field. There for it's not possible " +
                                    $"to have a {idGenericType.Name} table created for it.");
            }

            var genericType = idGenericType;
            
            if (idLeaf.PropertyAttributes.Any(a => a is TreatAsLeafAttribute))
            {
                genericType = idGenericForceLeafType;
            }
            
            var specifiedType = genericType.MakeGenericType(idLeaf.Type);

            return specifiedType;
        }
    }
}