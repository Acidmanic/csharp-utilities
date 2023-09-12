using System;
using System.Collections.Generic;
using Acidmanic.Utilities.DataTypes;
using Acidmanic.Utilities.Filtering.Models;
using Acidmanic.Utilities.Reflection;
using Acidmanic.Utilities.Reflection.ObjectTree;

namespace Acidmanic.Utilities.Filtering.Utilities
{
    public class OrderTermsComparer<T> : IComparer<T>
    {
        private readonly OrderTerm[] _orderTerms;
        private readonly ObjectEvaluator _evaluator;
        private readonly Type _entityType;

        private readonly Dictionary<Type, Func<object, object, int>> _comparers =
            new Dictionary<Type, Func<object, object, int>>();

        private readonly List<Type> _comparables = new List<Type>
        {
            typeof(string),
            typeof(int),
            typeof(long),
            typeof(short),
            typeof(byte),
            typeof(sbyte),
            typeof(decimal),
            typeof(float),
            typeof(ulong),
            typeof(uint),
            typeof(bool),
            typeof(char),
            typeof(DateTime),
            typeof(TimeStamp),
            typeof(TimeSpan),
        };

        public OrderTermsComparer(OrderTerm[] orderTerms)
        {
            _orderTerms = orderTerms;
            _entityType = typeof(T);
            _evaluator = new ObjectEvaluator(_entityType);
            foreach (var comparable in _comparables)
            {
                _comparers.Add(comparable, GetComparer(comparable));
            }
        }


        private Func<object, object, int> GetComparer(Type type)
        {
            if (type == typeof(string))
            {
                return (x, y) => String.Compare(((string)x), (string)y, StringComparison.Ordinal);
            }

            if (type == typeof(DateTime))
            {
                return (x, y) => Sign(((DateTime)x).Ticks - ((DateTime)y).Ticks);
            }

            if (type == typeof(TimeSpan))
            {
                return (x, y) => Sign(((TimeSpan)x).Ticks - ((TimeSpan)y).Ticks);
            }

            if (type == typeof(TimeStamp))
            {
                return (x, y) => Sign(((TimeStamp)x).TotalMilliSeconds - ((TimeStamp)y).TotalMilliSeconds);
            }

            if (type == typeof(bool))
            {
                return (x, y) =>
                {
                    var bx = (bool)x;
                    var by = (bool)y;

                    if (bx && !by)
                    {
                        return 1;
                    }

                    if (by && !bx)
                    {
                        return -1;
                    }

                    return 0;
                };
            }

            if (TypeCheck.IsNumerical(type))
            {
                return (x, y) => NumericalCompare.Compare(type, x, y);
            }

            return (x, y) => 0;
        }


        private int Sign(long value)
        {
            if (value == 0)
            {
                return 0;
            }

            return value > 0 ? 1 : -1;
        }


        public int Compare(T x, T y)
        {
            if (x == null && y == null)
            {
                return 0;
            }

            if (x == null)
            {
                return -1;
            }

            if (y == null)
            {
                return 1;
            }

            foreach (var term in _orderTerms)
            {
                var address = _evaluator.RootNode.Name + "." + term.Key;

                var node = _evaluator.Map.NodeByAddress(address);

                if (node != null && node.IsLeaf)
                {
                    var xObj = node.Evaluator.Read(x);
                    var yObj = node.Evaluator.Read(y);

                    var comp = CompareObject(xObj, yObj);

                    if (comp > 0)
                    {
                        return term.Sort == OrderSort.Ascending ? 1 : -1;
                    }

                    if (comp < 0)
                    {
                        return term.Sort == OrderSort.Ascending ? -1 : 1;
                    }
                }
            }

            return 0;
        }

        private int CompareObject(object xObj, object yObj)
        {
            var type = xObj.GetType();

            if (_comparers.ContainsKey(type))
            {
                return _comparers[type](xObj, yObj);
            }

            return 0;
        }
    }
}