using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoryBlog.Web.Blazor.Components;

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
