using System.Linq;
using System.Collections.Generic;
using Linq.Declarative.Tests.TestTypes;
using Linq.Declarative.Expression;
using Xunit;
using System;

namespace Linq.Declarative.Tests
{
    public class QueryComposerTest
    {
        private QueryExpressionComposer Composer { get; set; }
        
        public QueryComposerTest()
        {
            Composer = new QueryExpressionComposer();
        }

        [Fact]
        public void TestQueryWithNulledFilterReturnsEverything()
        {
            TestPlainFilter filter = new TestPlainFilter() { };
            IList<TestEntity> list = new List<TestEntity>()
                { 
                    new TestEntity() { Test2 = 1, Test3 = 3 },
                    new TestEntity() { Test2 = 1, Test3 = 4 }
                };
            IList<TestEntity> result = list.Where(filter).ToList();
            Assert.Equal(list.Count, result.Count);
        }

        [Fact]
        public void TestQueryByMultipleNullableBasicType()
        {
            TestPlainFilter filter = new TestPlainFilter() { Test2 = 1, Test3 = 4 };
            IList<TestEntity> list = new List<TestEntity>()
                { 
                    new TestEntity() { Test2 = 1, Test3 = 3 },
                    new TestEntity() { Test2 = 1, Test3 = 4 }
                };
            IList<TestEntity> result = list.Where(filter).ToList();
            Assert.Equal(1, result.Count);
            Assert.Equal(list[1], result[0]);
        }

        [Fact]
        public void TestQueryByNullableBasicType()
        {
            TestPlainFilter filter = new TestPlainFilter() { Test2 = 1 };
            IList<TestEntity> list = new List<TestEntity>()
                { 
                    new TestEntity() { Test2 = 1 },
                    new TestEntity() { Test2 = 2 }
                };
            IList<TestEntity> result = list.Where(filter).ToList();
            Assert.Equal(1, result.Count);
            Assert.Equal(list[0], result[0]);
        }

        [Fact]
        public void TestQueryByEnumInList()
        {
            TestAnnotatedFilter filter = new TestAnnotatedFilter() { TestEnumIn = new List<TestEnum>() { TestEnum.Test1 } };
            IList<TestEntity> list = new List<TestEntity>()
                { 
                    new TestEntity() { Test2 = 1, TestEnum = TestEnum.Test1 },
                    new TestEntity() { Test2 = 2, TestEnum = TestEnum.Test2 }
                };
            IList<TestEntity> result = list.Where(filter).ToList();
            Assert.Equal(1, result.Count);
            Assert.Equal(list[0], result[0]);
        }
        
        [Fact]
        public void TestQueryByNestedObject()
        {
            TestNestedFilter filter = new TestNestedFilter() { EntityId = 3 };
            TestEntity testEntity = new TestEntity() { Test2 = 1, TestEnum = TestEnum.Test1, Id = 0, Entity = new TestEntity() { Id = 3} };
            IList<TestEntity> list = new List<TestEntity>()
                { 
                    new TestEntity() { Test2 = 2, TestEnum = TestEnum.Test2, Id = 1 },
                    testEntity
                };
            IList<TestEntity> result = list.Where(filter).ToList();
            Assert.Equal(1, result.Count);
            Assert.Equal(testEntity, result[0]);
        }


        [Fact]
        public void TestQueryByNestedObjectWithStringLike()
        {
            TestNestedFilter filter = new TestNestedFilter() { Test1NestedLike = "Test" };
            TestEntity testEntity = new TestEntity() { Test2 = 1, TestEnum = TestEnum.Test1, Id = 0, Entity = new TestEntity() { Id = 3, Test1 = "Test1" } };
            IList<TestEntity> list = new List<TestEntity>()
                { 
                    new TestEntity() { Test2 = 2, TestEnum = TestEnum.Test2, Id = 1 },
                    testEntity
                };
            IList<TestEntity> result = list.Where(filter).ToList();
            Assert.Equal(1, result.Count);
            Assert.Equal(testEntity, result[0]);
        }

        [Fact]
        public void TestQueryByNestedObjectWithInCollection()
        {
            TestNestedFilter filter = new TestNestedFilter() { NestedInIds = new List<long>() { 3 } };
            TestEntity testEntity = new TestEntity() { Test2 = 1, TestEnum = TestEnum.Test1, Id = 0, Entity = new TestEntity() { Id = 3, Test1 = "Test1" } };
            IList<TestEntity> list = new List<TestEntity>()
                { 
                    new TestEntity() { Test2 = 2, TestEnum = TestEnum.Test2, Id = 1 },
                    testEntity
                };
            IList<TestEntity> result = list.Where(filter).ToList();
            Assert.Equal(1, result.Count);
            Assert.Equal(testEntity, result[0]);
        }

        [Fact]
        public void TestQueryWithNulledAnnotatedFilterReturnsEverything()
        {
            TestAnnotatedFilter filter = new TestAnnotatedFilter() { };
            IList<TestEntity> list = new List<TestEntity>()
                { 
                    new TestEntity() { Test2 = 1, Test3 = 3 },
                    new TestEntity() { Test2 = 1, Test3 = 4 }
                };
            IList<TestEntity> result = list.Where(filter).ToList();
            Assert.Equal(list.Count, result.Count);
        }

        [Fact]
        public void TestQueryByGreaterThanNullableBasicType()
        {
            TestAnnotatedFilter filter = new TestAnnotatedFilter() { Test2GreaterThan = 1 };
            IList<TestEntity> list = new List<TestEntity>()
                { 
                    new TestEntity() { Test2 = 1, Test3 = 3 },
                    new TestEntity() { Test2 = 1, Test3 = 4 }
                };
            IList<TestEntity> result = list.Where(filter).ToList();
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public void TestQueryByGreaterOrEqualThanNullableBasicType()
        {
            TestAnnotatedFilter filter = new TestAnnotatedFilter() { Test2GreaterOrEqualThan = 1 };
            IList<TestEntity> list = new List<TestEntity>()
                { 
                    new TestEntity() { Test2 = 1, Test3 = 3 },
                    new TestEntity() { Test2 = 1, Test3 = 4 }
                };
            IList<TestEntity> result = list.Where(filter).ToList();
            Assert.Equal(2, result.Count);
        }


        [Fact]
        public void TestQueryByLikeNullableBasicType()
        {
            TestAnnotatedFilter filter = new TestAnnotatedFilter() { Test1Like = "1" };
            IList<TestEntity> list = new List<TestEntity>()
                { 
                    new TestEntity() { Test1 = "123", Test2 = 1, Test3 = 3 },
                    new TestEntity() { Test2 = 1, Test3 = 4 }
                };
            IList<TestEntity> result = list.Where(filter).ToList();
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void TestQueryManyToManyByLikeBasicType()
        {
            TestManyToMany filter = new TestManyToMany() { Test1Like = "1" };
            IList<TestEntity> list = new List<TestEntity>()
                {
                    new TestEntity() { Entities = new List<TestEntity>() { new TestEntity() { Test1 = "1" } } }
                };
            IList<TestEntity> result = list.Where(filter).ToList();
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void TestQueryManyToManyByGreaterOrEqualThanBasicType()
        {
            TestManyToMany filter = new TestManyToMany() { TestDateTimeGreaterOrEqualThan = DateTime.Now };
            IList<TestEntity> list = new List<TestEntity>()
                {
                    new TestEntity() { Entities = new List<TestEntity>() { new TestEntity() { TestDateTime = DateTime.Now } } }
                };
            IList<TestEntity> result = list.Where(filter).ToList();
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void TestQueryManyToManyByGreaterThanBasicType()
        {
            TestManyToMany filter = new TestManyToMany() { TestDateTimeGreaterThan = DateTime.Now.AddDays(-1) };
            IList<TestEntity> list = new List<TestEntity>()
                {
                    new TestEntity() { Entities = new List<TestEntity>() { new TestEntity() { TestDateTime = DateTime.Now } } }
                };
            IList<TestEntity> result = list.Where(filter).ToList();
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void TestQueryManyToManyByLessOrEqualThanBasicType()
        {
            TestManyToMany filter = new TestManyToMany() { TestDateTimeLessOrEqualThan = DateTime.Now };
            IList<TestEntity> list = new List<TestEntity>()
                {
                    new TestEntity() { Entities = new List<TestEntity>() { new TestEntity() { TestDateTime = DateTime.Now } } }
                };
            IList<TestEntity> result = list.Where(filter).ToList();
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void TestQueryManyToManyByLessThanBasicType()
        {
            TestManyToMany filter = new TestManyToMany() { TestDateTimeLessThan = DateTime.Now.AddDays(1) };
            IList<TestEntity> list = new List<TestEntity>()
                {
                    new TestEntity() { Entities = new List<TestEntity>() { new TestEntity() { TestDateTime = DateTime.Now } } }
                };
            IList<TestEntity> result = list.Where(filter).ToList();
            Assert.Equal(1, result.Count);
        }
    }
}
