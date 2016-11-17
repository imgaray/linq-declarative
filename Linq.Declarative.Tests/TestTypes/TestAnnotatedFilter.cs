using Linq.Declarative.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq.Declarative.Tests.TestTypes
{
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
}
