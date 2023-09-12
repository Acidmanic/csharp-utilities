using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Acidmanic.Utilities.Filtering.Extensions;
using Acidmanic.Utilities.Filtering.Models;
using Acidmanic.Utilities.Reflection;

namespace Acidmanic.Utilities.Filtering.Utilities
{
    public class OrderSetBuilder<TStorage>
    {
        private readonly List<OrderTerm> _orderTerms = new List<OrderTerm>();


        public OrderSetBuilder<TStorage> OrderBy<T>(Expression<Func<TStorage, T>> selector, OrderSort sort)
        {
            //TODO: Support collections here by altering _selected values if node is collection
            //TODO: throw exception if node is not effectively a leaf
            var address = MemberOwnerUtilities.GetKey(selector).Headless().ToString();

            _orderTerms.Add(new OrderTerm
            {
                Key = address,
                Sort = sort
            });

            return this;
        }

        public OrderSetBuilder<TStorage> OrderAscendingBy<T>(Expression<Func<TStorage, T>> selector)
        {
            return this.OrderBy(selector, OrderSort.Ascending);
        }

        public OrderSetBuilder<TStorage> OrderDescendingBy<T>(Expression<Func<TStorage, T>> selector)
        {
            return this.OrderBy(selector, OrderSort.Descending);
        }


        public OrderTerm[] Build()
        {
            var terms = _orderTerms.ToArray();


            return terms;
        }
    }
}