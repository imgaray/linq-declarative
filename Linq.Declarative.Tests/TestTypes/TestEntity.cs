using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq.Declarative.Tests.TestTypes
{

    public enum TestEnum
    {
        Test1,
        Test2
    }

    public class TestEntity
    {
        public long Id { get; set; }

        public String Test1 { get; set; }
        public int Test2 { get; set; }
        public int Test3 { get; set; }
        public DateTime TestDateTime { get; set; }
        public TestEnum TestEnum { get; set; }

        public TestEntity Entity { get; set; }

        public List<TestEntity> Entities { get; set; }

        public override bool Equals(object other)
        {
            return other != null && other.GetType() == typeof(TestEntity) && ((TestEntity)other).Id == this.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
