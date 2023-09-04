using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using Acidmanic.Utilities.Filtering.Extensions;
using Acidmanic.Utilities.Reflection;
using Acidmanic.Utilities.Reflection.Extensions;
using Acidmanic.Utilities.Reflection.ObjectTree;
using Acidmanic.Utilities.Reflection.ObjectTree.FieldAddressing;

namespace Acidmanic.Utilities.Filtering.Utilities
{
    public class FilterQueryBuilder<TStorage>
    {
        private readonly ObjectEvaluator _evaluator;
        private string _selectedAddress = null;
        private FieldKey _selectedKey = null;
        private AccessNode _selectedNode = null;
        private bool _isAnyFieldSelected = false;

        private FilterQuery _filter;

        public FilterQueryBuilder()
        {
            _evaluator = new ObjectEvaluator(typeof(TStorage));

            Clear();
        }

        public void Clear()
        {
            _filter = new FilterQuery();
            
            _filter.EntityType = typeof(TStorage);

            _filter.Clear();
        }

        public FilterQueryBuilder<TStorage> Where<T>(Expression<Func<TStorage, T>> selector)
        {
            //TODO: Support collections here by altering _selected values if node is collection
            //TODO: throw exception if node is not effectively a leaf
            _selectedAddress = MemberOwnerUtilities.GetKey(selector).ToString();

            _selectedNode = _evaluator.Map.NodeByAddress(_selectedAddress);

            _selectedKey = _evaluator.Map.FieldKeyByAddress(_selectedAddress);


            _isAnyFieldSelected = true;

            return this;
        }

        public FilterQueryBuilder<TStorage> IsLargerThan(string minimum)
        {
            if (!_isAnyFieldSelected)
            {
                throw new Exception("You need to select a field first, by calling Where method.");
            }

            var filterItem = new FilterItem
            {
                Key = _selectedKey.Headless().ToString(),
                Minimum = minimum,
                EqualValues = new List<string>(),
                ValueComparison = ValueComparison.LargerThan,
                ValueType = _selectedNode.Type.GetAlteredOrOriginal()
            };

            _filter.Add(filterItem);

            return this;
        }

        public FilterQueryBuilder<TStorage> IsSmallerThan(string maximum)
        {
            if (!_isAnyFieldSelected)
            {
                throw new Exception("You need to select a field first, by calling Where method.");
            }

            var filterItem = new FilterItem
            {
                Key = _selectedKey.Headless().ToString(),
                Maximum = maximum,
                EqualValues = new List<string>(),
                ValueComparison = ValueComparison.SmallerThan,
                ValueType = _selectedNode.Type.GetAlteredOrOriginal()
            };

            _filter.Add(filterItem);

            return this;
        }

        public FilterQueryBuilder<TStorage> IsBetween(string minimum, string maximum)
        {
            if (!_isAnyFieldSelected)
            {
                throw new Exception("You need to select a field first, by calling Where method.");
            }

            var filterItem = new FilterItem
            {
                Key = _selectedKey.Headless().ToString(),
                Maximum = maximum,
                Minimum = minimum,
                EqualValues = new List<string>(),
                ValueComparison = ValueComparison.BetweenValues,
                ValueType = _selectedNode.Type.GetAlteredOrOriginal()
            };

            _filter.Add(filterItem);

            return this;
        }
        
        public FilterQueryBuilder<TStorage> IsEqualTo(params string[] equals)
        {
            if (!_isAnyFieldSelected)
            {
                throw new Exception("You need to select a field first, by calling Where method.");
            }

            var filterItem = new FilterItem
            {
                Key = _selectedKey.Headless().ToString(),
                EqualValues = new List<string>(),
                ValueComparison = ValueComparison.Equal,
                ValueType = _selectedNode.Type.GetAlteredOrOriginal()
            };
            
            foreach (var equal in equals)
            {
                filterItem.EqualValues.Add(equal);
            }

            _filter.Add(filterItem);

            return this;
        }


        public FilterQuery Build()
        {
            var filter = _filter;

            Clear();

            return filter;
        }
    }
}