using Linq.Declarative.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Linq.Declarative.Tests.TestTypes
{
    public class TestManyToMany
    {
        [Like(PropertyPath = "Entities.Test1")]
        public String Test1Like { get; set; }

        [GreaterThan(PropertyPath = "Entities.TestDateTime")]
        public DateTime? TestDateTimeGreaterThan { get; set; }

        [LessThan(PropertyPath = "Entities.TestDateTime")]
        public DateTime? TestDateTimeLessThan { get; set; }

        [GreaterOrEqualThan(PropertyPath = "Entities.TestDateTime")]
        public DateTime? TestDateTimeGreaterOrEqualThan { get; set; }

        [LessOrEqualThan(PropertyPath = "Entities.TestDateTime")]
        public DateTime? TestDateTimeLessOrEqualThan { get; set; }
    }
}
