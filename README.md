# linq-declarative
Linq extension for declarative querying based on reflection

## Usage

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

## Why use it
`Linq.Declarative` transforms filter object definitions to a compound `Linq.Expression`. As `IQueryable` interface can comprehend expressions for querying, we can use this expressions to easily construct queries that can be executed in `EntityFramework` (using `IDbSet` as `IQueryable`) or `NHibernate` (using `NHibernate.Linq`).

## How it works
As the Where method is invoked, a `QueryExpressionComposer` is instantiated and used to generate the expression. When the expression is correctly built, it is passed to the Where method implemented by `IQueryable`. You can use the Composer to generate the expression and use it elsewhere as well.

The way the Composer works is mainly by convention. It will try to match entities properties by name and type and the default behaviour is to compare by equal if that property has a value in the filter object. There are other custom operations:

Ordinal:
- GreaterThan
- GreaterOrEqualThan
- LessThan
- LessOrEqualThan
- NotEqual

String comparison:
- Like

Contained in subset:
- In

Each definition can also be hinted to which attribute to target. Eg:
```C#
   [In(PropertyPath = "Entity.Id")]
   public List<long> NestedInIds { get; set; }
```

This would try to navigate the target instance to get `queriedObject.Entity.Id` and then would apply `nestedIds.Contains(queriedObject.Entity.Id)`.

All operations with proyections have nullchecks enabled by default, but this behaviour can be overriden at attribute level:
```C#
    [In(PropertyPath = "Entity.Id", EnableNullCheckQuery = false)]
    public List<long> NestedInIds { get; set; }
```

And that's all the magic behind the scenes.

## Future releases
Next releases will decouple the query composer from the attribute usage, for it to be able to generate queries from different origins, not only compiled classes. This would allow us to generate multiple input processors that generate the same output.

