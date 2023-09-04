using System.Linq.Expressions;
using Acidmanic.Utilities.Filtering;
using Acidmanic.Utilities.Filtering.Attributes;
using Acidmanic.Utilities.Filtering.Utilities;
using Acidmanic.Utilities.Reflection;
using Acidmanic.Utilities.Reflection.Attributes;

namespace Acidmanic.Utilities.Test.Unit
{
    public class FilterQueryIneMemoryFilteringTests
    {
        [FilterResultExpirationDuration(1000)]
        private class StorageModel
        {
            [FilterField] public string Name { get; set; }
            [FilterField] public string Surname { get; set; }
            [UniqueMember] [AutoValuedMember] public long Id { get; set; }
            [FilterField] public int Age { get; set; }
            [FilterField] public int Height { get; set; }
        }


        private static readonly int ManiId = 1;
        private static readonly int MonaId = 2;
        private static readonly int MinaId = 3;
        private static readonly int FarshidId = 4;
        
        
        private List<StorageModel> CreateTestData()
        {
            return new List<StorageModel>
            {
                new StorageModel
                {
                    Age = 37,
                    Height = 175,
                    Id = ManiId,
                    Name = "Mani",
                    Surname = "Moayedi"
                },
                new StorageModel
                {
                    Age = 41,
                    Height = 170,
                    Id = MonaId,
                    Name = "Mona",
                    Surname = "Moayedi"
                },
                new StorageModel
                {
                    Age = 50,
                    Height = 170,
                    Id = MinaId,
                    Name = "Mina",
                    Surname = "Haddadi"
                },
                new StorageModel
                {
                    Age = 60,
                    Height = 175,
                    Id = FarshidId,
                    Name = "Farshid",
                    Surname = "Moayedi"
                },
            };
        }

        private FilterQuery CreateBetweenNumeralFilterQuery<TProperty>(Expression<Func<StorageModel,TProperty>> selector,string min, string max)
        {
            var key = MemberOwnerUtilities.GetKey(selector).TerminalSegment().Name;
            
            var filter = new FilterQuery();

            filter.EntityType = typeof(StorageModel);
            
            filter.Add(new FilterItem
            {
                Key = key,
                Maximum = max,
                Minimum = min,
                ValueComparison = ValueComparison.BetweenValues,
                ValueType = typeof(TProperty)
            });

            return filter;
        }
        
        private FilterQuery CreateLargerThanFilterQuery<TProperty>(Expression<Func<StorageModel,TProperty>> selector,string min)
        {
            var key = MemberOwnerUtilities.GetKey(selector).TerminalSegment().Name;
            
            var filter = new FilterQuery();

            filter.EntityType = typeof(StorageModel);
            
            filter.Add(new FilterItem
            {
                Key = key,
                Minimum = min,
                ValueComparison = ValueComparison.LargerThan,
                ValueType = typeof(TProperty)
            });

            return filter;
        }
        
        private FilterQuery CreateSmallerThanFilterQuery<TProperty>(Expression<Func<StorageModel,TProperty>> selector,string max)
        {
            var key = MemberOwnerUtilities.GetKey(selector).TerminalSegment().Name;
            
            var filter = new FilterQuery();

            filter.EntityType = typeof(StorageModel);
            
            filter.Add(new FilterItem
            {
                Key = key,
                Maximum = max,
                ValueComparison = ValueComparison.SmallerThan,
                ValueType = typeof(TProperty)
            });

            return filter;
        }
        
        private FilterQuery CreateEqualFilterQuery<TProperty>(Expression<Func<StorageModel,TProperty>> selector, params string[] values)
        {

            var key = MemberOwnerUtilities.GetKey(selector).TerminalSegment().Name;
            
            var filter = new FilterQuery();

            filter.EntityType = typeof(StorageModel);
            
            filter.Add(new FilterItem
            {
                Key = key,
                ValueComparison = ValueComparison.Equal,
                EqualValues = new List<string>(values),
                ValueType = typeof(TProperty)
            });

            return filter;
        }

        [Fact]
        public void FiltererMustMatchMonaAndMina()
        {
            var data = CreateTestData();

            var builder = new FilterQueryBuilder<StorageModel>();

            var filter = builder.Where(p => p.Age).IsBetween("40", "50").Build();
            
            var sut = new ObjectStreamFilterer<StorageModel,long>();

            var result = sut.PerformFilter(data, filter,filter.Hash());
            
            Assert.Equal(2,result.Count);
            
            Assert.Equal(MonaId,result[0].ResultId);
            
            Assert.Equal(MinaId,result[1].ResultId);
        }
        
        [Fact]
        public void FiltererMustMatchAll4Records()
        {
            var data = CreateTestData();

            var filter = CreateBetweenNumeralFilterQuery(r=>r.Age,"10","80");

            var sut = new ObjectStreamFilterer<StorageModel,long>();

            var result = sut.PerformFilterByHash(data, filter);
            
            Assert.Equal(4,result.Count);
            
        }
        
        [Fact]
        public void FiltererMustMatchMinaAndFarshid()
        {
            var data = CreateTestData();

            var filter = CreateLargerThanFilterQuery( m=>m.Age, "45");

            var sut = new ObjectStreamFilterer<StorageModel,long>();

            var result = sut.PerformFilterByHash(data, filter);
            
            Assert.Equal(2,result.Count);
            
            Assert.Equal(MinaId,result[0].ResultId);
            
            Assert.Equal(FarshidId,result[1].ResultId);
        }
        
        [Fact]
        public void FiltererMustMatchManiAndMona()
        {
            var data = CreateTestData();

            var filter = CreateSmallerThanFilterQuery( m=> m.Age,"45");

            var sut = new ObjectStreamFilterer<StorageModel,long>();

            var result = sut.PerformFilterByHash(data, filter);
            
            Assert.Equal(2,result.Count);
            
            Assert.Equal(ManiId,result[0].ResultId);
            
            Assert.Equal(MonaId,result[1].ResultId);
        }
        
        [Fact]
        public void FiltererMustMatchMona()
        {
            var data = CreateTestData();

            var filter = CreateEqualFilterQuery( m => m.Age,"41");

            var sut = new ObjectStreamFilterer<StorageModel,long>();

            var result = sut.PerformFilterByHash(data, filter);
            
            Assert.Equal(1,result.Count);
            
            Assert.Equal(MonaId,result[0].ResultId);
            
        }
        
        [Fact]
        public void FiltererMustMatchMonaAndMani()
        {
            var data = CreateTestData();

            var filter = CreateEqualFilterQuery( m => m.Age,"41","37");

            var sut = new ObjectStreamFilterer<StorageModel,long>();

            var result = sut.PerformFilterByHash(data, filter);
            
            Assert.Equal(2,result.Count);
            
            Assert.Equal(ManiId,result[0].ResultId);
            
            Assert.Equal(MonaId,result[1].ResultId);
            
        }
        
        [Fact]
        public void FiltererMustMatchMonaAndMinaByName()
        {
            var data = CreateTestData();

            var filter = CreateEqualFilterQuery( m => m.Name,"Mona","Mina");

            var sut = new ObjectStreamFilterer<StorageModel,long>();

            var result = sut.PerformFilterByHash(data, filter);
            
            Assert.Equal(2,result.Count);
            
            Assert.Equal(MonaId,result[0].ResultId);
            
            Assert.Equal(MinaId,result[1].ResultId);
            
        }
        
        [Fact]
        public void FiltererMustMatchMoayedies()
        {
            var data = CreateTestData();

            var filter = CreateEqualFilterQuery( m => m.Surname,"Moayedi");

            var sut = new ObjectStreamFilterer<StorageModel,long>();

            var result = sut.PerformFilterByHash(data, filter);
            
            Assert.Equal(3,result.Count);
            
            Assert.Equal(ManiId,result[0].ResultId);
            
            Assert.Equal(MonaId,result[1].ResultId);
            
            Assert.Equal(FarshidId,result[2].ResultId);
            
        }
        
        [Fact]
        public void FiltererMustMatchLargerThanMNames()
        {
            var data = CreateTestData();

            var filter = CreateLargerThanFilterQuery( m => m.Name,"M");

            var sut = new ObjectStreamFilterer<StorageModel,long>();

            var result = sut.PerformFilterByHash(data, filter);
            
            Assert.Equal(3,result.Count);
            
            Assert.Equal(ManiId,result[0].ResultId);
            
            Assert.Equal(MonaId,result[1].ResultId);
            
            Assert.Equal(MinaId,result[2].ResultId);
            
        }
        
        [Fact]
        public void FiltererMustMatchSmallerThanG()
        {
            var data = CreateTestData();

            var filter = CreateSmallerThanFilterQuery( m => m.Name,"G");

            var sut = new ObjectStreamFilterer<StorageModel,long>();

            var result = sut.PerformFilterByHash(data, filter);
            
            Assert.Equal(1,result.Count);
            
            Assert.Equal(FarshidId,result[0].ResultId);
            
        }
        
        [Fact]
        public void FiltererMustMatchBetweenMaaaAndMjjj()
        {
            var data = CreateTestData();

            var filter = CreateBetweenNumeralFilterQuery( m => m.Name,"Maaa","Mjjj");

            var sut = new ObjectStreamFilterer<StorageModel,long>();

            var result = sut.PerformFilterByHash(data, filter);
            
            Assert.Equal(2,result.Count);
            
            Assert.Equal(ManiId,result[0].ResultId);
            Assert.Equal(MinaId,result[1].ResultId);
            
        }
    }
}