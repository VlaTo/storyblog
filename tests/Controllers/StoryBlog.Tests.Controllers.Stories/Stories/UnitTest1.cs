using System;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StoryBlog.Tests.Controllers.Stories.Stories
{
    [TestClass]
    public sealed class UnitTest1 : TestBase
    {
        private MyEnum value;
        private string valueString;

        protected override void Arrange()
        {
        }

        protected override void Act()
        {
            value = MyEnum.One | MyEnum.Two;
            valueString = value.ToString();
        }

        [TestMethod]
        public void TestMethod1()
        {
            Assert.ThrowsException<InvalidEnumArgumentException>(() => { Enum.IsDefined(typeof(MyEnum), valueString); });
        }

        [Flags]
        private enum MyEnum : byte
        {
            One = 0x01,
            Two = 0x02,
            Four = 0x04
        }
    }
}
