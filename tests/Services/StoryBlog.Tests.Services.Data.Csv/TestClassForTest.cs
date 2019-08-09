using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoryBlog.Web.Blazor.Components;
using StoryBlog.Web.Services.Blog.Interop.Core;
using StoryBlog.Web.Services.Blog.Interop.Core.Annotations;

namespace StoryBlog.Tests.Services.Data.Csv
{
    [TestClass]
    public class TestClassForTest
    {
        [TestMethod]
        public void Test1()
        {
            var button = new ButtonComponent
            {
                IsOutline = true,
                IsActive = true,
                Type = BootstrapButtonTypes.Secondary,
                Size = BootstrapButtonSizes.Large
            };
            var classNameBuilder = new ClassBuilder<ButtonComponent>("btn")
                .DefineClass(@class => @class
                    .Modifier("outline", component => component.IsOutline)
                    .Name(component => EnumHelper.GetClassName(component.Type))
                    .Condition(component => BootstrapButtonTypes.Default != component.Type)
                )
                .DefineClass(@class => @class.Name("lg").Condition(component => BootstrapButtonSizes.Large == component.Size))
                .DefineClass(@class => @class.Name("sm").Condition(component => BootstrapButtonSizes.Small == component.Size))
                .DefineClass(@class => @class.NoPrefix().Name("active").Condition(component => component.IsActive));
            var style = classNameBuilder.Build(button, ((IComponentClassExposure) button).Class);
        }

        [TestMethod]
        public void Test2()
        {
            //var include = EnumFlags.ToQueryString(TestFlags.Value1 | TestFlags.Value3);
            var value = String.Join(CultureInfo.InvariantCulture.TextInfo.ListSeparator, "value-1", "Value3");
            var temp = Flags.Parse(typeof(TestFlags), value);
            //Assert.IsNotNull(include);
        }

        [TestMethod]
        public void Test3()
        {
            var value = (TestFlags.Value1 | TestFlags.Value2).ToString("F");
            //var include = EnumFlags.ToQueryString(TestFlags.Value1 | TestFlags.Value2);
            //var temp = EnumFlags.Parse<TestFlags>(new[] {"value-1", "Value3"}, StringComparer.InvariantCulture);
            //Assert.IsNotNull(include);
        }
    }

    [Flags]
    public enum TestFlags
    {
        [Flag(Key = "value-1")]
        Value1 = 1,

        [Flag(Key = "value-2")]
        Value2 = 2,

        [Flag]
        Value3 = 4
    }

    public interface IComponentClassExposure
    {
        string Class
        {
            get;
        }
    }

    public class ButtonComponent : BootstrapComponentBase, IComponentClassExposure
    {
        public BootstrapButtonTypes Type
        {
            get;
            set;
        }

        public BootstrapButtonSizes Size
        {
            get;
            set;
        }

        public bool IsActive
        {
            get;
            set;
        }

        public bool IsOutline
        {
            get;
            set;
        }

        string IComponentClassExposure.Class => Class;
    }
}
