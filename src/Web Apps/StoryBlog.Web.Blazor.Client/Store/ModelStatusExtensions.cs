namespace StoryBlog.Web.Blazor.Client.Store
{
    public static class ModelStatusExtensions
    {
        public static bool IsNone(this ModelStatus status)
        {
            return ModelStatus.None.Equals(status);
        }

        public static bool IsFailed(this ModelStatus status) => status == null && ModelState.Error == status.State;

        public static bool IsLoading(this ModelStatus status) => ModelStatus.Loading.Equals(status);
    }
}