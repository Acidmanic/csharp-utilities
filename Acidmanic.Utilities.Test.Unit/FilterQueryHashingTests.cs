using Acidmanic.Utilities.Filtering;

namespace Acidmanic.Utilities.Test.Unit
{
    public class FilterQueryHashingTests
    {
        [Fact]
        public void SimilarSQueriesMustProduceSameHashes()
        {
            var sq1 = CreateQuery1();

            var sq2 = CreateQuery1();

            var hash1 = sq1.Hash();

            var hash2 = sq2.Hash();

            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void SimilarSQueriesWithDifferentOrdersMustProduceSameHashes()
        {
            var sq1 = CreateQuery1();

            var sq2 = CreateQuery1OrderAltered();

            var hash1 = sq1.Hash();

            var hash2 = sq2.Hash();

            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void DifferentSQueriesWithMustProduceDifferentHashes()
        {
            var sq1 = CreateQuery1();

            var sq2 = CreateQuery2();

            var hash1 = sq1.Hash();

            var hash2 = sq2.Hash();

            Assert.NotEqual(hash1, hash2);
        }


        private class Mani { }
        private class Mona { }

        [Fact]
        public void SimilarQueriesWithDifferentTypessMustProduceDifferentHashes()
        {
            var sq1 = CreateQuery1();
            sq1.EntityType = typeof(Mani);
            var sq2 = CreateQuery1();
            sq1.EntityType = typeof(Mona);
            var hash1 = sq1.Hash();

            var hash2 = sq2.Hash();

            Assert.NotEqual(hash1, hash2);
        }


        private FilterQuery CreateQuery1()
        {
            var query = new FilterQuery();

            query.Add(new FilterItem
            {
                Key = "Name",
                EqualValues = new List<string> { "Mani", "Mona", "Mina", "Farshid" },
                ValueComparison = ValueComparison.Equal,
                ValueType = typeof(string)
            });

            query.Add(new FilterItem
            {
                Key = "Age",
                Maximum = "100",
                Minimum = "0",
                ValueComparison = ValueComparison.BetweenValues,
                ValueType = typeof(int)
            });

            query.Add(new FilterItem
            {
                Key = "Height",
                Minimum = "150",
                ValueComparison = ValueComparison.LargerThan,
                ValueType = typeof(int)
            });

            return query;
        }

        private FilterQuery CreateQuery1OrderAltered()
        {
            var query = new FilterQuery();


            query.Add(new FilterItem
            {
                Key = "Age",
                Maximum = "100",
                Minimum = "0",
                ValueComparison = ValueComparison.BetweenValues,
                ValueType = typeof(int)
            });

            query.Add(new FilterItem
            {
                Key = "Height",
                Minimum = "150",
                ValueComparison = ValueComparison.LargerThan,
                ValueType = typeof(int)
            });

            query.Add(new FilterItem
            {
                Key = "Name",
                EqualValues = new List<string> { "Mani", "Mona", "Mina", "Farshid" },
                ValueComparison = ValueComparison.Equal,
                ValueType = typeof(string)
            });


            return query;
        }

        private FilterQuery CreateQuery2()
        {
            var query = new FilterQuery();

            query.Add(new FilterItem
            {
                Key = "Brand",
                EqualValues = new List<string> { "Nokia", "Samsung", "Sony" },
                ValueComparison = ValueComparison.Equal,
                ValueType = typeof(string)
            });

            query.Add(new FilterItem
            {
                Key = "Rate",
                Minimum = "2.5",
                ValueComparison = ValueComparison.LargerThan,
                ValueType = typeof(double)
            });

            query.Add(new FilterItem
            {
                Key = "Battery",
                Minimum = "1500",
                ValueComparison = ValueComparison.LargerThan,
                ValueType = typeof(int)
            });

            return query;
        }
    }
}