using Linq.Declarative.Filters;
using System.Collections.Generic;

namespace Linq.Declarative.Tests.TestTypes
{
    public class TestNestedFilter
    {
        [Equals(PropertyPath = "Entity.Id")]
        public long? EntityId { get; set; }

        [Like(PropertyPath = "Entity.Test1")]
        public string Test1NestedLike { get; set; }

        [In(PropertyPath = "Entity.Id")]
        public IList<long> NestedInIds { get; set; }
    }
}
