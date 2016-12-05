# linq-declarative
Linq extension for declarative querying based on reflection

# Usage

First, you define your filter class
```C#
public class TestAnnotatedFilter
    {
        [Like]
        public String Test1Like { get; set; }

        [GreaterThan]
        public int? Test2GreaterThan { get; set; }

        [LessThan]
        public int? Test2LessThan { get; set; }

        [GreaterOrEqualThan]
        public int? Test2GreaterOrEqualThan { get; set; }

        [LessOrEqualThan]
        public int? Test2LessOrEqualThan { get; set; }


        public int? Test3 { get; set; }

        [In]
        public List<TestEnum> TestEnumIn { get; set; }
    }
```

Then you query your collection
```C#
  # list is a predefined collection
  TestAnnotatedFilter filter = new TestAnnotatedFilter() { TestEnumIn = new List<TestEnum>() { TestEnum.Test1 } };
  IList<TestEntity> result = list.Where(filter).ToList();
```

And that's pretty much it.

# Why use it
Linq.Declarative transforms filter object definitions to a compound Linq.Expression. As IQueryable interface can comprehend expressions for querying, we can use this expressions to easily construct queries that can be executed in EntityFramework (using IDbSet as IQueryable) or NHibernate (using NHibernate.Linq).
