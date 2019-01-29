using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StoryBlog.Tests.Services.Data.Csv
{
    public abstract class TestBase
    {
        [TestInitialize]
        public async Task Setup()
        {
            await ArrangeAsync();
            await ActAsync();
        }

        [TestCleanup]
        public virtual void Cleanup()
        {
        }

        protected abstract Task ArrangeAsync();

        protected abstract Task ActAsync();
    }
}
