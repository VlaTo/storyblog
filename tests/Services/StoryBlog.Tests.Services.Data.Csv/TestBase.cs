using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StoryBlog.Tests.Services.Data.Csv
{
    public abstract class TestBase
    {
        [TestInitialize]
        public void Setup()
        {
            Arrange();
            Act();
        }

        [TestCleanup]
        public virtual void Cleanup()
        {
        }

        protected abstract void Arrange();

        protected abstract void Act();
    }
}
