using Linq.Declarative.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq.Declarative.Tests.TestTypes
{
    public class TestObjectCollectionFilter
    {
        [In]
        public List<TestEntity> EntityIn { get; set; }
    }
}
