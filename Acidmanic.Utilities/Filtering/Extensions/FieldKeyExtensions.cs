using System.Linq;
using Acidmanic.Utilities.Reflection.ObjectTree.FieldAddressing;

namespace Acidmanic.Utilities.Filtering.Extensions
{
    public static class FieldKeyExtensions
    {


        public static Segment GetHead(this FieldKey key)
        {
            return key.FirstOrDefault();
        }


        public static FieldKey Headless(this FieldKey fieldKey)
        {
            if (fieldKey.Count > 0)
            {
                var headless = new FieldKey();

                for (int i = 1; i < fieldKey.Count; i++)
                {
                    headless.Add(fieldKey[i]);
                }

                return headless;
            }

            return fieldKey;
        }
    }
}