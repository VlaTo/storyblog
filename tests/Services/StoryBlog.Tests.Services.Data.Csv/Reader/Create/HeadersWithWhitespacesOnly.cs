using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StoryBlog.Tests.Services.Data.Csv.Reader.Create
{
    [TestClass]
    public sealed class HeadersWithWhitespacesOnly : ContextBase
    {
        //protected override string Text => "test,test1, test2 , test3,=\"030810023\"";
        protected override string Text => "test";

        [TestMethod]
        public void Test1()
        {
            var enumType = typeof(TestEnum);
            var value = (object) TestEnum.Second;

            Assert.IsTrue(Enum.IsDefined(enumType, value));

            var name = Enum.GetName(enumType, value);
            var property = enumType.GetField(name, BindingFlags.Static | BindingFlags.Public);
            //Debug.WriteLine($"[EnumHelper.GetClassName] property: {property}");
            //var attribute = property?.GetCustomAttribute<StyleAttribute>();
            //Assert.IsNotNull(Document);
        }

        protected override Task ActAsync()
        {
            return Task.CompletedTask;
        }
    }

    public enum TestEnum
    {
        First,
        Second
    }
}