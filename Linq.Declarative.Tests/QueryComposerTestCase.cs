using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Linq.Declarative.Expression;
using Linq.Declarative.Tests.TestTypes;

namespace Linq.Declarative.Tests
{
    [TestClass]
    public class QueryComposerTestCase
    {
        private QueryExpressionComposer Composer { get; set; }

        [TestInitialize]
        public void SetUp()
        {
            Composer = new QueryExpressionComposer();
        }

        [TestMethod]
        public void TestQueryWithNulledFilterReturnsEverything()
        {
            TestPlainFilter filter = new TestPlainFilter() { };
            IList<TestEntity> list = new List<TestEntity>()
                { 
                    new TestEntity() { Test2 = 1, Test3 = 3 },
                    new TestEntity() { Test2 = 1, Test3 = 4 }
                };
            IList<TestEntity> result = list.Where(filter).ToList();
            Assert.AreEqual(list.Count, result.Count);
        }

        [TestMethod]
        public void TestQueryByMultipleNullableBasicType()
        {
            TestPlainFilter filter = new TestPlainFilter() { Test2 = 1, Test3 = 4 };
            IList<TestEntity> list = new List<TestEntity>()
                { 
                    new TestEntity() { Test2 = 1, Test3 = 3 },
                    new TestEntity() { Test2 = 1, Test3 = 4 }
                };
            IList<TestEntity> result = list.Where(filter).ToList();
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(list[1], result[0]);
        }

        [TestMethod]
        public void TestQueryByNullableBasicType()
        {
            TestPlainFilter filter = new TestPlainFilter() { Test2 = 1 };
            IList<TestEntity> list = new List<TestEntity>()
                { 
                    new TestEntity() { Test2 = 1 },
                    new TestEntity() { Test2 = 2 }
                };
            IList<TestEntity> result = list.Where(filter).ToList();
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(list[0], result[0]);
        }

        [TestMethod]
        public void TestQueryByEnumInList()
        {
            TestAnnotatedFilter filter = new TestAnnotatedFilter() { TestEnumIn = new List<TestEnum>() { TestEnum.Test1 } };
            IList<TestEntity> list = new List<TestEntity>()
                { 
                    new TestEntity() { Test2 = 1, TestEnum = TestEnum.Test1 },
                    new TestEntity() { Test2 = 2, TestEnum = TestEnum.Test2 }
                };
            IList<TestEntity> result = list.Where(filter).ToList();
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(list[0], result[0]);
        }
        
        [TestMethod]
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
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(testEntity, result[0]);
        }


        [TestMethod]
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
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(testEntity, result[0]);
        }

        [TestMethod]
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
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(testEntity, result[0]);
        }

        [TestMethod]
        public void TestQueryWithNulledAnnotatedFilterReturnsEverything()
        {
            TestAnnotatedFilter filter = new TestAnnotatedFilter() { };
            IList<TestEntity> list = new List<TestEntity>()
                { 
                    new TestEntity() { Test2 = 1, Test3 = 3 },
                    new TestEntity() { Test2 = 1, Test3 = 4 }
                };
            IList<TestEntity> result = list.Where(filter).ToList();
            Assert.AreEqual(list.Count, result.Count);
        }

        [TestMethod]
        public void TestQueryByGreaterThanNullableBasicType()
        {
            TestAnnotatedFilter filter = new TestAnnotatedFilter() { Test2GreaterThan = 1 };
            IList<TestEntity> list = new List<TestEntity>()
                { 
                    new TestEntity() { Test2 = 1, Test3 = 3 },
                    new TestEntity() { Test2 = 1, Test3 = 4 }
                };
            IList<TestEntity> result = list.Where(filter).ToList();
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void TestQueryByGreaterOrEqualThanNullableBasicType()
        {
            TestAnnotatedFilter filter = new TestAnnotatedFilter() { Test2GreaterOrEqualThan = 1 };
            IList<TestEntity> list = new List<TestEntity>()
                { 
                    new TestEntity() { Test2 = 1, Test3 = 3 },
                    new TestEntity() { Test2 = 1, Test3 = 4 }
                };
            IList<TestEntity> result = list.Where(filter).ToList();
            Assert.AreEqual(2, result.Count);
        }


        [TestMethod]
        public void TestQueryByLikeNullableBasicType()
        {
            TestAnnotatedFilter filter = new TestAnnotatedFilter() { Test1Like = "1" };
            IList<TestEntity> list = new List<TestEntity>()
                { 
                    new TestEntity() { Test1 = "123", Test2 = 1, Test3 = 3 },
                    new TestEntity() { Test2 = 1, Test3 = 4 }
                };
            IList<TestEntity> result = list.Where(filter).ToList();
            Assert.AreEqual(1, result.Count);
        }
    }
}
